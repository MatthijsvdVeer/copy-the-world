param principalIds array
param keyVaultName string

resource keyVault 'Microsoft.KeyVault/vaults@2021-11-01-preview' existing = {
  name: keyVaultName
}

var keyVaultSecretsUser = '4633458b-17de-408a-b874-0445c86b69e6'
@description('Key Vault Secrets User')
resource contributorRoleDefinition 'Microsoft.Authorization/roleDefinitions@2018-01-01-preview' existing = {
  scope: subscription()
  name: keyVaultSecretsUser
}

resource roleAssignment 'Microsoft.Authorization/roleAssignments@2020-04-01-preview' = [for principalId in principalIds: {
  scope: keyVault
  name: guid(keyVault.id, principalId, keyVaultSecretsUser)
  properties: {
    roleDefinitionId: contributorRoleDefinition.id
    principalId: principalId
    principalType: 'ServicePrincipal'
  }
}]
