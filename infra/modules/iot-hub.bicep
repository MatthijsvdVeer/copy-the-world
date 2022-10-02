param applicationName string
param location string = resourceGroup().location

@secure()
param telemetryHubConnection string

resource iotHub 'Microsoft.Devices/IotHubs@2021-07-02' = {
  name: 'iot-${applicationName}-${uniqueString(resourceGroup().id)}'
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  sku: {
    capacity: 1
    name: 'S1'
  }
  properties: {
    disableDeviceSAS: false
    disableLocalAuth: true
    disableModuleSAS: false
    routing: {
      fallbackRoute: {
        endpointNames: [
          'events'
        ]
        source: 'DeviceMessages'
        isEnabled: true
      }
      endpoints: {
        eventHubs: [
          {
            name: 'telemetry'
            connectionString: telemetryHubConnection
          }
        ]
      }
      routes: [
        {
          name: 'all'
          endpointNames: [
            'telemetry'
          ]
          source: 'DeviceMessages'
          isEnabled: true
        }
      ]
    }
  }
}

resource defender 'Microsoft.Security/iotSecuritySolutions@2019-08-01' = {
  name: iotHub.name
  location: location
  properties: {
    status: 'Enabled'
    unmaskedIpLoggingStatus: 'Disabled'
    displayName: iotHub.name
    iotHubs: [
      iotHub.id
    ]
  }
}

output iotHubName string = iotHub.name
