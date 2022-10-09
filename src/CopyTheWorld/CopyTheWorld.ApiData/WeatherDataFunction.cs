namespace CopyTheWorld.ApiData;

using System;
using Azure.DigitalTwins.Core;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Linq;

public class WeatherDataFunction
{
    private readonly DigitalTwinsClient digitalTwinsClient;

    public WeatherDataFunction(DigitalTwinsClient digitalTwinsClient) => this.digitalTwinsClient = digitalTwinsClient;

    [FunctionName("WeatherDataFunction")]
    public void Run(
        [TimerTrigger("0 */5 * * * *")]TimerInfo myTimer,
        [EventHub("patches", Connection = "PatchesSend")] IAsyncCollector<string> outputEvents,
        ILogger log)
    {
        const string query = "SELECT city.$dtId FROM DIGITALTWINS city WHERE IS_OF_MODEL(city, 'dtmi:digitaltwins:ctw:City;1')";
        var cities = digitalTwinsClient.Query<BasicDigitalTwin>(query);

        var cityIds = cities.Select(twin => twin.Id);
        var cityAndWeather = 
    }
}