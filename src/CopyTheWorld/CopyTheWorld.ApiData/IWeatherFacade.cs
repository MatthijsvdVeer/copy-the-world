namespace CopyTheWorld.ApiData
{
    using Shared.TwinModels;

    public interface IWeatherFacade
    {
        Weather GetWeatherForBuilding(Building building);
    }
}