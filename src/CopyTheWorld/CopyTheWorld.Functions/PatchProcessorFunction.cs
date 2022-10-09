namespace CopyTheWorld.Functions;

using System;
using System.Threading.Tasks;
using Azure;
using Azure.DigitalTwins.Core;
using Azure.Messaging.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Shared;
using System.Text.Json;

public sealed class PatchProcessorFunction
{
    private readonly DigitalTwinsClient digitalTwinsClient;

    public PatchProcessorFunction(DigitalTwinsClient digitalTwinsClient) =>
        this.digitalTwinsClient = digitalTwinsClient;

    [FunctionName("PatchProcessorFunction")]
    public async Task Run([EventHubTrigger("patches", Connection = "PatchesListen", ConsumerGroup = "function")] EventData eventData, ILogger log)
    {
        log.LogInformation($"C# Event Hub trigger function processed a message: {eventData.EventBody}");
        var twinPatch = await JsonSerializer.DeserializeAsync<TwinPatch>(eventData.BodyAsStream);

        try
        {
            await this.UpdateDigitalTwinAsync(twinPatch, PatchType.Replace);
        }
        catch (RequestFailedException e)
        {
            if (e.Status != 400 ||
                !string.Equals(e.ErrorCode, "JsonPatchInvalid", StringComparison.Ordinal))
            {
                throw;
            }

            await this.UpdateDigitalTwinAsync(twinPatch, PatchType.Add);
        }
    }

    private async Task UpdateDigitalTwinAsync(TwinPatch twinPatch, PatchType patchType)
    {
        var jsonPatchDocument = new JsonPatchDocument();
        switch (patchType)
        {
            case PatchType.Replace:
                {
                    jsonPatchDocument.AppendReplace(twinPatch.Property, twinPatch.Value);
                    break;
                }
            case PatchType.Add:
                {
                    jsonPatchDocument.AppendAdd(twinPatch.Property, twinPatch.Value);
                    break;
                }
            case PatchType.None:
            default:
                {
                    throw new ArgumentOutOfRangeException(nameof(patchType), patchType, null);
                }
        }

        _ = await this.digitalTwinsClient.UpdateDigitalTwinAsync(twinPatch.TwinId, jsonPatchDocument);
    }
}