namespace CopyTheWorld.Provisioning.Twins.Populate.TwinBuilders;

using Azure.DigitalTwins.Core;
using CopyTheWorld.Provisioning;
using CopyTheWorld.Shared.TwinModels;
using System.Data;

internal sealed class Co2AirQualitySensorBuilder : ITwinBuilder<Co2AirQualitySensor>
{
    public (Co2AirQualitySensor, BasicRelationship) CreateTwinAndRelationship(DataRow dataRow)
    {
        var id = dataRow.GetStringValue("ID");
        var target = dataRow.GetStringValue("Target");
        var airQualitySensor = new Co2AirQualitySensor { Id = id, Name = "CO2 Sensor", LastValue = -1, ExternalIds = { ["deviceId"] = id } };
        var relationship = TwinUtility.GetRelationshipFor(id, "observes", target);
        return (airQualitySensor, relationship);
    }
}