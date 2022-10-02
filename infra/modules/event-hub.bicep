param eventHubNamespaceName string
param hubName string
param keyVaultName string
param extraConsumerGroup string

resource keyvault 'Microsoft.KeyVault/vaults@2021-11-01-preview' existing = {
  name: keyVaultName
}

resource eventHubNamespace 'Microsoft.EventHub/namespaces@2021-11-01'  existing = {
  name: eventHubNamespaceName
}

resource eventHub 'Microsoft.EventHub/namespaces/eventhubs@2021-11-01' = {
  name: hubName
  parent: eventHubNamespace
  properties: {
    messageRetentionInDays: 1
    partitionCount: 1
    status: 'Active'
  }
}

resource defaultConsumerGroup 'Microsoft.EventHub/namespaces/eventhubs/consumergroups@2021-11-01' = {
  name: '$Default'
  parent: eventHub
}

resource additionalConsumerGroup 'Microsoft.EventHub/namespaces/eventhubs/consumergroups@2021-11-01' = {
  name: extraConsumerGroup
  parent: eventHub
}


resource authorizationRuleListen 'Microsoft.EventHub/namespaces/eventhubs/authorizationRules@2021-11-01' = {
  name: 'Listen'
  parent: eventHub
  properties: {
    rights: [
      'Listen'
    ]
  }
}

resource listenSecret 'Microsoft.KeyVault/vaults/secrets@2021-11-01-preview' = {
  name: 'connection-${hubName}-listen'
  parent: keyvault
  properties: {
    value: authorizationRuleListen.listKeys().primaryConnectionString
  }
}

resource authorizationRuleSend 'Microsoft.EventHub/namespaces/eventhubs/authorizationRules@2021-11-01' = {
  name: 'Send'
  parent: eventHub
  properties: {
    rights: [
      'Send'
    ]
  }
}

resource sendSecret 'Microsoft.KeyVault/vaults/secrets@2021-11-01-preview' = {
  name: 'connection-${hubName}-send'
  parent: keyvault
  properties: {
    value: authorizationRuleSend.listKeys().primaryConnectionString
  }
}

output listenSecretName string = listenSecret.name
output listenSecretUrl string = listenSecret.properties.secretUriWithVersion
output sendSecretName string = sendSecret.name
output sendSecretUrl string = sendSecret.properties.secretUriWithVersion
