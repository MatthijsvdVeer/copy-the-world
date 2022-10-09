param applicationName string
param location string = resourceGroup().location

resource adt 'Microsoft.DigitalTwins/digitalTwinsInstances@2021-06-30-preview' = {
  name: 'adt-${applicationName}-${uniqueString(resourceGroup().id)}'
  location: location
  identity: {
    type: 'SystemAssigned'
  }
}

output azureDigitalTwinsName string = adt.name
output azureDigitalTwinsEndpoint string = adt.properties.hostName
