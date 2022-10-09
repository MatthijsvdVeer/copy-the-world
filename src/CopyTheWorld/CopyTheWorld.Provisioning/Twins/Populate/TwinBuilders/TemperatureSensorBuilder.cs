namespace CopyTheWorld.Provisioning.Twins.Populate.TwinBuilders;

using Azure.DigitalTwins.Core;
using CopyTheWorld.Provisioning;
using CopyTheWorld.Shared.TwinModels;
using System.Data;

internal sealed class TemperatureSensorBuilder : ITwinBuilder<TemperatureSensor>
{
    public (TemperatureSensor, BasicRelationship) CreateTwinAndRelationship(DataRow dataRow)
    {
        var id = dataRow.GetStringValue("ID");
        var target = dataRow.GetStringValue("Target");
        var temperatureSensor = new TemperatureSensor { Id = id, Name = "Temperature Sensor", LastValue = -1, ExternalIds = { ["deviceId"] = id } };
        var relationship = TwinUtility.GetRelationshipFor(id, "observes", target);
        return (temperatureSensor, relationship);
    }
}