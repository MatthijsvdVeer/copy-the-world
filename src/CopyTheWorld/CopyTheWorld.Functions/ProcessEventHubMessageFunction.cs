namespace CopyTheWorld.Functions;

using Azure.Data.Tables;
using System.Text.Json.Nodes;
using System;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Shared;
using System.Text.Json;
using System.Linq;

public sealed class ProcessEventHubMessageFunction
{
    private const string id = "iothub-connection-device-id";
    private readonly TableClient tableClient;

    public ProcessEventHubMessageFunction(TableClient tableClient) => this.tableClient = tableClient;

    [FunctionName("ProcessEventHubMessageFunction")]
    public async Task Run(
        [EventHubTrigger("ingress", Connection = "IngressListen", ConsumerGroup = "function")] EventData eventData,
        [EventHub("patches", Connection = "PatchesSend")]
        IAsyncCollector<string> outputEvents,
        ILogger log)
    {
        try
        {
            var rowKey = eventData.SystemProperties[id].ToString();
            var response = this.tableClient.GetEntity<MappingEntity>(rowKey, rowKey);
            var mappingDefinitions = JsonSerializer.Deserialize<MappingDefinition[]>(response.Value.Mapping);
        
            var jsonNode = JsonNode.Parse(eventData.EventBody);
            foreach (var mappingDefinition in mappingDefinitions)
            {
                var propertyNode = GetJsonNodeByPath(jsonNode, mappingDefinition.Property);
                var value = GetValueFromNode(mappingDefinition, propertyNode);

                var twinPatch = new TwinPatch
                {
                    Value = value,
                    Property = mappingDefinition.TwinProperty,
                    TwinId = mappingDefinition.TwinId
                };

                var message = JsonSerializer.Serialize(twinPatch);
                log.LogInformation(message);
                await outputEvents.AddAsync(message);
            }
        }
        catch (Exception exception)
        {
            log.LogError(exception, $"Exception occured while parsing {eventData.EventBody}");
            throw;
        }
    }

    private static object GetValueFromNode(MappingDefinition mappingDefinition, JsonNode propertyNode) =>
        mappingDefinition.DataType switch
        {
            "string" => propertyNode.GetValue<string>(),
            "int" => propertyNode.GetValue<int>(),
            "double" => propertyNode.GetValue<double>(),
            "bool" => propertyNode.GetValue<bool>(),
            _ => throw new ArgumentOutOfRangeException(
                $"Type of {mappingDefinition.DataType} not supported.")
        };

    private static JsonNode GetJsonNodeByPath(JsonNode node, string path)
    {
        var segments = path.Split('/');
        return segments.Aggregate(node, (current, segment) => current[segment]);
    }
}