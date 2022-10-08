param principalIds array
param iotHubName string

resource iotHub 'Microsoft.Devices/IotHubs@2021-07-02' existing = {
  name: iotHubName
}

var adtDataOwner = '4fc6c259-987e-4a07-842e-c321cc9d413f'
@description('This is the built-in IoT Hub Data Contributor role.')
resource contributorRoleDefinition 'Microsoft.Authorization/roleDefinitions@2018-01-01-preview' existing = {
  scope: subscription()
  name: adtDataOwner
}

resource roleAssignment 'Microsoft.Authorization/roleAssignments@2020-04-01-preview' = [for principalId in principalIds: {
  scope: iotHub
  name: guid(iotHub.id, principalId, adtDataOwner)
  properties: {
    roleDefinitionId: contributorRoleDefinition.id
    principalId: principalId
    principalType: 'ServicePrincipal'
  }
}]
