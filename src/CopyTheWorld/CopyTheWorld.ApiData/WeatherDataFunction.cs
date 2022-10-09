namespace CopyTheWorld.ApiData;

using Azure.DigitalTwins.Core;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Linq;
using Shared;
using System.Text.Json;
using System.Threading.Tasks;

public class WeatherDataFunction
{
    private readonly DigitalTwinsClient digitalTwinsClient;

    private readonly IWeatherFacade weatherFacade;

    public WeatherDataFunction(DigitalTwinsClient digitalTwinsClient, IWeatherFacade weatherFacade)
    {
        this.digitalTwinsClient = digitalTwinsClient;
        this.weatherFacade = weatherFacade;
    }

    [FunctionName("WeatherDataFunction")]
    public async Task Run(
        [TimerTrigger("0 */5 * * * *")]TimerInfo myTimer,
        [EventHub("patches", Connection = "PatchesSend")] IAsyncCollector<string> outputEvents,
        ILogger log)
    {
        // Limitation, can't JOIN with $dtId filter.
        const string query = "SELECT city.$dtId FROM DIGITALTWINS city WHERE IS_OF_MODEL(city, 'dtmi:digitaltwins:ctw:City;1')";
        var cities = this.digitalTwinsClient.Query<BasicDigitalTwin>(query);

        var cityIds = cities.Select(twin => $"'{twin.Id}'");
        const string queryTemplate = @"
SELECT city, weather.$dtId
FROM DIGITALTWINS city
JOIN weather RELATED weather.capabilityOf
WHERE city.$dtId IN[{0}]
";
        var cityAndWeatherQuery = string.Format(queryTemplate, string.Join(',', cityIds));
        var results = this.digitalTwinsClient.Query<WeatherAndCityResult>(cityAndWeatherQuery);
        foreach (var weatherAndCityResult in results)
        {
            var weather = this.weatherFacade.GetWeatherForCity(weatherAndCityResult.City);
            var conditionsPatch = new TwinPatch
            {
                Value = weather.Conditions,
                Property = "/conditions",
                TwinId = weatherAndCityResult.Weather.Id
            };

            await SendPatch(outputEvents, log, conditionsPatch);

            var temperaturePatch = new TwinPatch
            {
                Value = weather.Temperature,
                Property = "/temperature",
                TwinId = weatherAndCityResult.Weather.Id
            };

            await SendPatch(outputEvents, log, temperaturePatch);
        }
    }

    private static async Task SendPatch(IAsyncCollector<string> outputEvents, ILogger log, TwinPatch twinPatch)
    {
        var message = JsonSerializer.Serialize(twinPatch);
        log.LogInformation(message);
        await outputEvents.AddAsync(message);
    }
}