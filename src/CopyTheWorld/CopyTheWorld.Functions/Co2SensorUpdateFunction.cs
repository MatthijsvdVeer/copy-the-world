namespace CopyTheWorld.Functions;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Azure.Messaging.EventGrid;
using Azure.DigitalTwins.Core;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Text.Json;

public class Co2SensorUpdateFunction
{
    private readonly DigitalTwinsClient digitalTwinsClient;

    public Co2SensorUpdateFunction(DigitalTwinsClient digitalTwinsClient) =>
        this.digitalTwinsClient = digitalTwinsClient;

    [FunctionName("Co2SensorUpdateFunction")]
    public async Task Run(
        [EventGridTrigger] EventGridEvent eventGridEvent,
        [EventHub("patches", Connection = "PatchesSend")]
        IAsyncCollector<string> outputEvents,
        ILogger log)
    {
        log.LogInformation($"Received message for {eventGridEvent.Subject}");
        try
        {
            var twinId = eventGridEvent.Subject;
            var twinUpdate = eventGridEvent.Data.ToObjectFromJson<TwinUpdate>();
            var occupied = twinUpdate.Data.Patches.SingleOrDefault(patch => string.Equals(patch.Path, "/lastValue"));
            if (occupied == null)
            {
                log.LogInformation("Co2 hasn't changed.");
                return;
            }

            const string query = @"
SELECT space.$dtId
FROM DIGITALTWINS sensors
JOIN space RELATED sensors.observes
WHERE sensors.$dtId = '{0}'";

            var twins = this.digitalTwinsClient.Query<BasicDigitalTwin>(string.Format(query, twinId));
            foreach (var twin in twins)
            {
                var twinPatch = new TwinPatch { Value = occupied.Value, Property = "/CO2/CO2Sensor", TwinId = twin.Id };

                var message = JsonSerializer.Serialize(twinPatch);
                log.LogInformation(message);
                await outputEvents.AddAsync(message);
            }
        }
        catch (Exception exception)
        {
            log.LogError(exception, $"Error while executing with data: {eventGridEvent.Data}");
            throw;
        }
    }
}