param eventGridTopicName string
param dtmi string
param functionAppName string
param functionName string

resource topic 'Microsoft.EventGrid/topics@2022-06-15' existing = {
  name: eventGridTopicName
}
resource functionApp 'Microsoft.Web/sites@2020-06-01' existing = {
  name: functionAppName
}

resource function 'Microsoft.Web/sites/functions@2022-03-01' existing = {
  name: functionName
  parent: functionApp
}

resource subscription 'Microsoft.EventGrid/eventSubscriptions@2022-06-15' = {
  name: 'subscription-${functionName}'
  scope: topic
  properties: {
    filter: {
      advancedFilters: [
        {
          operatorType: 'StringIn'
          key: 'data.data.modelId'
          values: [
            dtmi
          ]
        }
      ]
    }
    destination: {
      endpointType: 'AzureFunction'
      properties: {
        maxEventsPerBatch: 1
        preferredBatchSizeInKilobytes: 64
        resourceId: function.id
      }
    }
  }
}
