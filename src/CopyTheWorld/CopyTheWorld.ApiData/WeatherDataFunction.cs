namespace CopyTheWorld.ApiData;

using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

public class WeatherDataFunction
{
    [FunctionName("WeatherDataFunction")]
    public void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
    {
        log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
    }
}