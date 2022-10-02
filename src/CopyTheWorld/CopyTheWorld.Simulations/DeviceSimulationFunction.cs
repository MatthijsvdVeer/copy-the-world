using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace CopyTheWorld.Simulations
{
    public class DeviceSimulationFunction
    {
        [FunctionName("DeviceSimulationFunction")]
        public void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
