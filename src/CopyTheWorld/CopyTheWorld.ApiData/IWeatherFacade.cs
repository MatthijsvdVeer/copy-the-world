namespace CopyTheWorld.ApiData
{
    using Shared.TwinModels;

    public interface IWeatherFacade
    {
        Weather GetWeatherForCity(City city);
    }
}