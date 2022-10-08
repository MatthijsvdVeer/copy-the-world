namespace CopyTheWorld.Provisioning.Populate.TwinBuilders;

using Azure.DigitalTwins.Core;
using CopyTheWorld.Provisioning;
using Shared.TwinModels;
using System.Data;

internal sealed class TemperatureSensorBuilder : ITwinBuilder<TemperatureSensor>
{
    public (TemperatureSensor, BasicRelationship) CreateTwinAndRelationship(DataRow dataRow)
    {
        var id = dataRow.GetStringValue("ID");
        var target = dataRow.GetStringValue("Target");
        var motionSensor = new TemperatureSensor { Id = id, LastValue = -1, ExternalIds = { ["deviceId"] = id } };
        var relationship = TwinUtility.GetRelationshipFor(id, "observes", target);
        return (motionSensor, relationship);
    }
}