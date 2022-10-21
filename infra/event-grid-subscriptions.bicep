param functionAppName string
param eventGridTopicName string
param subscriptions array

module subscription 'modules/event-grid-topic-subscription.bicep' = [for item in subscriptions: {
  name: guid(item.functionName)
  params: {
    dtmi: item.modelId
    eventGridTopicName: eventGridTopicName
    functionAppName: functionAppName
    functionName: item.functionName
  }
}]
