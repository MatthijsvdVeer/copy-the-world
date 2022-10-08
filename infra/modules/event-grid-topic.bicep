param applicationName string
param location string = resourceGroup().location

resource topic 'Microsoft.EventGrid/topics@2022-06-15' = {
  name: 'eg--${applicationName}-${uniqueString(resourceGroup().id)}'
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    inputSchema: 'EventGridSchema'
    publicNetworkAccess: 'Enabled'
    disableLocalAuth: false
    dataResidencyBoundary: 'WithinGeopair'
  }
}

output name string = topic.name
