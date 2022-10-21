param applicationName string = 'ctw'
param principalId string
param tenantId string
param location string = resourceGroup().location
param functionsName string = 'ctwFunction'
param functionsSimulationName string = 'ctwSimulations'
param functionsApiDataName string = 'ctwApiData'

resource keyVault 'Microsoft.KeyVault/vaults@2021-11-01-preview' = {
  name: 'kv-${uniqueString(resourceGroup().id)}'
  location: location
  properties: {
    enabledForTemplateDeployment: true
    sku: {
      name: 'standard'
      family: 'A'
    }
    tenantId: tenantId
    softDeleteRetentionInDays: 90
    enableSoftDelete: true
    enableRbacAuthorization: true
  }
}

var secretsOfficerPrincipalIds = [
  principalId
]

module keyVaultSecretsOfficers 'modules/key-vault-secrets-officer.bicep' = {
  name: 'key-vault-secrets-officer'
  params: {
    keyVaultName: keyVault.name
    principalIds: secretsOfficerPrincipalIds
  }
}

module eventHubNamespace 'modules/event-hub-namespace.bicep' = {
  name: 'event-hub-namespace'
  params: {
    applicationName: applicationName
    location: location
  }
}

module patchesHub 'modules/event-hub.bicep' = {
  name: 'patches-hub'
  params: {
    eventHubNamespaceName: eventHubNamespace.outputs.eventHubNamespaceName
    extraConsumerGroup: 'function'
    hubName: 'patches'
    keyVaultName: keyVault.name
  }
}

module ingressHub 'modules/event-hub.bicep' = {
  name: 'ingress-hub'
  params: {
    eventHubNamespaceName: eventHubNamespace.outputs.eventHubNamespaceName
    extraConsumerGroup: 'function'
    hubName: 'ingress'
    keyVaultName: keyVault.name
  }
}

resource storageAccount 'Microsoft.Storage/storageAccounts@2021-09-01' = {
  name: 'st${toLower(take(applicationName,8))}${uniqueString(resourceGroup().id)}'
  location: location
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
  }
  properties: {
    supportsHttpsTrafficOnly: true
    minimumTlsVersion: 'TLS1_2'
  }
  resource tableService 'tableServices@2021-09-01' = {
    name: 'default'
  }
}

module mappingTable 'modules/table-storage.bicep' = {
  name: 'mapping-table'
  params: {
    storageAccountName: storageAccount.name
    tableName: 'mapping'
  }
}

module appInsights 'modules/application-insights.bicep' = {
  name: 'application-insights'
  params: {
    functionFullName: functionsName
    simulationsFullName: functionsSimulationName
    apiDataFullName: functionsApiDataName
    applicationName: applicationName
    location: location
  }
}

module hostingPlan 'modules/hosting-plan.bicep' = {
  name: 'hosting-plan'
  params: {
    applicationName: applicationName
    location: location
  }
}

module functions 'modules/function.bicep' = {
  name: 'functions'
  params: {
    hostingPlanName: hostingPlan.outputs.planName
    functionFullName: functionsName
    storageAccountName: storageAccount.name
    applicationInsightsInstrumentationKey: appInsights.outputs.instrumentationKey
    location: location
  }
}

module simulationFunction 'modules/function.bicep' = {
  name: 'simulation-function'
  params: {
    hostingPlanName: hostingPlan.outputs.planName
    functionFullName: functionsSimulationName
    storageAccountName: storageAccount.name
    applicationInsightsInstrumentationKey: appInsights.outputs.instrumentationKey
    location: location
  }
}

module apiDataFunction 'modules/function.bicep' = {
  name: 'api-data-function'
  params: {
    hostingPlanName: hostingPlan.outputs.planName
    functionFullName: functionsApiDataName
    storageAccountName: storageAccount.name
    applicationInsightsInstrumentationKey: appInsights.outputs.instrumentationKey
    location: location
  }
}

var tableDataContributorPrincipalIds = [
  functions.outputs.principalId
  principalId
]

module tableDataContributors 'modules/table-storage-data-contributor.bicep' = {
  name: 'table-storage-data-contributor'
  params: {
    principalIds: tableDataContributorPrincipalIds
    storageAccountName: storageAccount.name
  }
}

module iotHub 'modules/iot-hub.bicep' = {
  name: 'iot-hub'
  params:{
    applicationName: applicationName
    location: location
    telemetryHubConnection: keyVault.getSecret(ingressHub.outputs.sendSecretName)
  }
}

var iotHubDataContributorPrincipalIds = [
  principalId
]

module iotHubDataContributors 'modules/iot-hub-data-contributor.bicep' = {
  name: 'iot-hub-data-contributors'
  params: {
    iotHubName: iotHub.outputs.iotHubName
    principalIds: iotHubDataContributorPrincipalIds
  }
}

module adt 'modules/azure-digital-twins.bicep' = {
  name: 'azure-digital-twins'
  params: {
    applicationName: applicationName
    location: location
  }
}

var principalsForAzureDigitalTwins = [
  functions.outputs.principalId
  simulationFunction.outputs.principalId
  apiDataFunction.outputs.principalId
  principalId
]

module adtAccess 'modules/azure-digital-twins-data-owner.bicep' = {
  name: 'adt-data-owner'
  params: {
    adtName: adt.outputs.azureDigitalTwinsName
    principalIds: principalsForAzureDigitalTwins
  }
}

var secretsUserPrincipalIds = [
  functions.outputs.principalId
  simulationFunction.outputs.principalId
  apiDataFunction.outputs.principalId
]

module keyVaultSecretsUsers 'modules/key-vault-secrets-user.bicep' = {
  name: 'key-vault-secrets-users'
  params: {
    keyVaultName: keyVault.name
    principalIds: secretsUserPrincipalIds
  }
}

module eventGridTopic 'modules/event-grid-topic.bicep' = {
  name: 'event-grid-topic'
  params: {
    applicationName: applicationName
    location: location
  }
}

module adtEndpoint 'modules/azure-digital-twins-eg-endpoint.bicep' = {
  name: 'adt-endpoint'
  params: {
    endpointName: 'changes'
    adtName: adt.outputs.azureDigitalTwinsName
    topicName: eventGridTopic.outputs.name
  }
}

output patchesListenSecretUrl string = patchesHub.outputs.listenSecretUrl
output patchesSendSecretUrl string = patchesHub.outputs.sendSecretUrl
output ingressListenSecretUrl string = ingressHub.outputs.listenSecretUrl
output ingressSendSecretUrl string = ingressHub.outputs.sendSecretUrl
output apiDataAppName string = apiDataFunction.outputs.functionName
output simulationAppName string = simulationFunction.outputs.functionName
output functionAppName string =  functions.outputs.functionName
output azureDigitalTwinsEndpoint string = adt.outputs.azureDigitalTwinsEndpoint
output tableEndpoint string = storageAccount.properties.primaryEndpoints.table
output azureDigitalTwinsName string = adt.outputs.azureDigitalTwinsName
output eventGridTopicName string = eventGridTopic.outputs.name
