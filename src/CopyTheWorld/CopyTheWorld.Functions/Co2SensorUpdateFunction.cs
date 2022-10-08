namespace CopyTheWorld.Functions;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Azure.Messaging.EventGrid;
using Azure.DigitalTwins.Core;
using System.Threading.Tasks;

public class Co2SensorUpdateFunction
{
    private readonly DigitalTwinsClient digitalTwinsClient;

    public Co2SensorUpdateFunction(DigitalTwinsClient digitalTwinsClient) => this.digitalTwinsClient = digitalTwinsClient;

    [FunctionName("Co2SensorUpdateFunction")]
    public async Task Run(
        [EventGridTrigger]EventGridEvent eventGridEvent,
        [EventHub("patches", Connection = "PatchesSend")] IAsyncCollector<string> outputEvents,
        ILogger log)
    {
        log.LogInformation($"Received message for {eventGridEvent.Subject}");
    }
}