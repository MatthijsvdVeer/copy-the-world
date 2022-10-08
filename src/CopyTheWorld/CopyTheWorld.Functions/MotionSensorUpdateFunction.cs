namespace CopyTheWorld.Functions;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Azure.Messaging.EventGrid;
using Azure.DigitalTwins.Core;
using System;
using System.Text.Json;
using System.Linq;
using System.Threading.Tasks;

public class MotionSensorUpdateFunction
{
    private readonly DigitalTwinsClient digitalTwinsClient;

    public MotionSensorUpdateFunction(DigitalTwinsClient digitalTwinsClient) => this.digitalTwinsClient = digitalTwinsClient;

    [FunctionName("MotionSensorUpdateFunction")]
    public async Task Run(
        [EventGridTrigger]EventGridEvent eventGridEvent,
        [EventHub("patches", Connection = "PatchesSend")] IAsyncCollector<string> outputEvents,
        ILogger log)
    {
        try
        {
            var twinId = eventGridEvent.Subject;
            var twinUpdate = eventGridEvent.Data.ToObjectFromJson<TwinUpdate>();
            var occupied = twinUpdate.Data.Patches.SingleOrDefault(patch => string.Equals(patch.Path, "/lastValue"));
            if (occupied == null)
            {
                log.LogInformation("Occupancy hasn't changed.");
                return;
            }
        
            const string query = @"
SELECT space
FROM DIGITALTWINS sensors
JOIN space RELATED sensors.observes
WHERE sensors.$dtId = '{0}'";

            var twins = this.digitalTwinsClient.Query<BasicDigitalTwin>(string.Format(query, twinId));
            foreach (var twin in twins)
            {
                var twinPatch = new TwinPatch
                {
                    Value = occupied.Value,
                    Property = "/occupancy/occupied",
                    TwinId = twin.Id
                };

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