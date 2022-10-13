namespace CopyTheWorld.ApiData
{
    using Shared.TwinModels;
    using System;

    internal sealed class WeatherFacade : IWeatherFacade
    {
        private readonly Random random;

        private static readonly string[] conditions = new[] { "sunny", "rain", "snow" };

        public WeatherFacade() => this.random = new Random();

        public Weather GetWeatherForBuilding(Building building) =>
            new()
            {
                Conditions = conditions[this.random.Next(conditions.Length)], Temperature = this.random.Next(0, 11)
            };
    }
}