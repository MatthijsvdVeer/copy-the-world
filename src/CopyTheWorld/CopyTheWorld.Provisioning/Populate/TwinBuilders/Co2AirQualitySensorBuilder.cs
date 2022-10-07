namespace CopyTheWorld.Provisioning.Populate.TwinBuilders;

using Azure.DigitalTwins.Core;
using DigitalTwin.Provisioning;
using Shared.TwinModels;
using System.Data;

internal sealed class Co2AirQualitySensorBuilderBuilder : ITwinBuilder<Co2AirQualitySensor>
{
    public (Co2AirQualitySensor, BasicRelationship) CreateTwinAndRelationship(DataRow dataRow)
    {
        var id = dataRow.GetStringValue("ID");
        var target = dataRow.GetStringValue("Target");
        var motionSensor = new Co2AirQualitySensor { Id = id, LastValue = -1, ExternalIds = { ["deviceId"] = id } };
        var relationship = TwinUtility.GetRelationshipFor(id, "observes", target);
        return (motionSensor, relationship);
    }
}