namespace CopyTheWorld.ApiData
{
    using Azure.DigitalTwins.Core;
    using Shared.TwinModels;

    internal sealed class WeatherAndBuildingResult
    {
        public Building Building { get; set; }

        public BasicDigitalTwin Weather { get; set; }
    }
}