namespace CopyTheWorld.Provisioning.Twins.Populate;

using Azure.DigitalTwins.Core;
using CopyTheWorld.Shared;
using System.Data;
using System.Linq;
using TwinBuilders;

internal sealed class TwinFactory
{
    public CreateTwinsAndRelationshipsResult CreateTwinsAndRelationships(DataTable dataTable) =>
        dataTable.TableName switch
        {
            "Planets" => GetObjects(new PlanetBuilder(), dataTable),
            "Countries" => GetObjects(new CountryBuilder(), dataTable),
            "Cities" => GetObjects(new CityBuilder(), dataTable),
            "Buildings" => GetObjects(new BuildingBuilder(), dataTable),
            "Weather" => GetObjects(new WeatherBuilder(), dataTable),
            "Levels" => GetObjects(new LevelBuilder(), dataTable),
            "Rooms" => GetObjects(new RoomBuilder(), dataTable),
            "PhoneBooths" => GetObjects(new PhoneBoothBuilder(), dataTable),
            "Desks" => GetObjects(new FurnitureBuilder(), dataTable),
            "Zones" => GetObjects(new ZoneBuilder(), dataTable),
            "Parking Lots" => GetObjects(new ParkingLotBuilder(), dataTable),
            "Parking spots" => GetObjects(new ParkingSpotBuilder(), dataTable),
            "RoomMotionSensors" => GetObjects(new MotionSensorBuilder(), dataTable),
            "DeskMotionSensors" => GetObjects(new MotionSensorBuilder(), dataTable),
            "Co2Sensors" => GetObjects(new Co2AirQualitySensorBuilder(), dataTable),
            "TemperatureSensors" => GetObjects(new TemperatureSensorBuilder(), dataTable),
            _ => new CreateTwinsAndRelationshipsResult()
        };

    private static CreateTwinsAndRelationshipsResult GetObjects<T>(ITwinBuilder<T> builder, DataTable dataTable) where T : BasicDigitalTwin
    {
        var tuples = dataTable.Rows.Cast<DataRow>().Select(builder.CreateTwinAndRelationship);
        var twins = tuples.Select(tuple => tuple.Item1.SetModelOnTwin()).ToList();
            
        var basicRelationships = tuples.Where(tuple => tuple.Item2 != TwinUtility.EmptyRelation).Select(tuple => tuple.Item2).ToList();
        return new CreateTwinsAndRelationshipsResult(twins, basicRelationships);
    }
}