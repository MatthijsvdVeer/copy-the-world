param adtName string
param topicName string

resource adt 'Microsoft.DigitalTwins/digitalTwinsInstances@2021-06-30-preview' existing = {
  name: adtName
}

resource topic 'Microsoft.EventGrid/topics@2022-06-15' existing = {
  name: topicName
}

resource endpoint 'Microsoft.DigitalTwins/digitalTwinsInstances/endpoints@2022-05-31' = {
  name: 'changes'
  parent: adt
  properties: {
    TopicEndpoint: topic.properties.endpoint
    accessKey1: topic.listKeys().key1
    endpointType: 'EventGrid'
  }
}

