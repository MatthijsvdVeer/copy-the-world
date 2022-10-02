param location string = resourceGroup().location
param applicationName string

resource eventHubNamespace 'Microsoft.EventHub/namespaces@2021-11-01' = {
  name: 'eh-${applicationName}-${uniqueString(resourceGroup().id)}'
  location: location
  sku: {
    name: 'Standard'
    tier: 'Standard'
    capacity: 1
  }
  properties: {
    disableLocalAuth: false
    zoneRedundant: true
  }
}

resource authorizationRule 'Microsoft.EventHub/namespaces/authorizationRules@2021-11-01' = {
  name: '${eventHubNamespace.name}/RootManageSharedAccessKey'
  properties: {
    rights: [
      'Listen'
      'Manage'
      'Send'
    ]
  }
}

output eventHubNamespaceName string = eventHubNamespace.name
