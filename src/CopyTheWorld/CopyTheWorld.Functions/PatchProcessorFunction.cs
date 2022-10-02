using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace CopyTheWorld.Functions;

public static class PatchProcessorFunction
{
    [FunctionName("PatchProcessorFunction")]
    public static async Task Run([EventHubTrigger("patches", Connection = "PatchesConnection")] EventData eventData, ILogger log)
    {
        await Task.Delay(0);
    }
}