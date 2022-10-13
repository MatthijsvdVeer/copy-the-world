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
        #region Get the cities
        // Limitation, can't JOIN with $dtId filter.
        const string query = @"
SELECT building.$dtId FROM DIGITALTWINS building 
WHERE IS_OF_MODEL(building, 'dtmi:digitaltwins:ctw:Building;1')";

        var buildings = this.digitalTwinsClient.Query<BasicDigitalTwin>(query);
        #endregion

        #region Get weather capabilities

        var buildingIds = buildings.Select(twin => $"'{twin.Id}'");
        const string queryTemplate = @"
SELECT building, weather.$dtId
FROM DIGITALTWINS building
JOIN weather RELATED weather.capabilityOf
WHERE building.$dtId IN[{0}]
";
        var buildingAndWeather = string.Format(queryTemplate, string.Join(',', buildingIds));
        var results = this.digitalTwinsClient.Query<WeatherAndBuildingResult>(buildingAndWeather);
        
        #endregion

        #region Patch the weather

        foreach (var weatherAndBuildingResult in results)
        {
            var weather = this.weatherFacade.GetWeatherForBuilding(weatherAndBuildingResult.Building);
            var conditionsPatch = new TwinPatch
            {
                Value = weather.Conditions,
                Property = "/conditions",
                TwinId = weatherAndBuildingResult.Weather.Id
            };

            await SendPatch(outputEvents, log, conditionsPatch);

            var temperaturePatch = new TwinPatch
            {
                Value = weather.Temperature,
                Property = "/temperature",
                TwinId = weatherAndBuildingResult.Weather.Id
            };

            await SendPatch(outputEvents, log, temperaturePatch);
        }

        #endregion
    }

    private static async Task SendPatch(IAsyncCollector<string> outputEvents, ILogger log, TwinPatch twinPatch)
    {
        var message = JsonSerializer.Serialize(twinPatch);
        log.LogInformation(message);
        await outputEvents.AddAsync(message);
    }
}