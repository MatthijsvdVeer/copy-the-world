using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace CopyTheWorld.Functions;

public static class ProcessEventHubMessageFunction
{
    [FunctionName("ProcessEventHubMessageFunction")]
    public static async Task Run([EventHubTrigger("ingress", Connection = "")] EventData eventData, ILogger log)
    {
        await Task.Delay(0);
    }
}