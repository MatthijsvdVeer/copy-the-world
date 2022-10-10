namespace CopyTheWorld.ApiData
{
    using Azure.DigitalTwins.Core;
    using Shared.TwinModels;

    internal sealed class WeatherAndCityResult
    {
        public City City { get; set; }

        public BasicDigitalTwin Weather { get; set; }
    }
}