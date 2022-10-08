param principalIds array
param adtName string

resource adt 'Microsoft.DigitalTwins/digitalTwinsInstances@2021-06-30-preview' existing = {
  name: adtName
}

var adtDataOwner = 'bcd981a7-7f74-457b-83e1-cceb9e632ffe'
@description('This is the built-in Azure Digital Twins Data Owner role.')
resource contributorRoleDefinition 'Microsoft.Authorization/roleDefinitions@2018-01-01-preview' existing = {
  scope: subscription()
  name: adtDataOwner
}

resource roleAssignment 'Microsoft.Authorization/roleAssignments@2020-04-01-preview' = [for principalId in principalIds: {
  scope: adt
  name: guid(adt.id, principalId, adtDataOwner)
  properties: {
    roleDefinitionId: contributorRoleDefinition.id
    principalId: principalId
    principalType: 'ServicePrincipal'
  }
}]
