namespace CopyTheWorld.Provisioning.Twins.Populate.TwinBuilders;

using Azure.DigitalTwins.Core;
using CopyTheWorld.Provisioning;
using CopyTheWorld.Shared.TwinModels;
using System.Data;

internal sealed class MotionSensorBuilder : ITwinBuilder<MotionSensor>
{
    public (MotionSensor, BasicRelationship) CreateTwinAndRelationship(DataRow dataRow)
    {
        var id = dataRow.GetStringValue("ID");
        var target = dataRow.GetStringValue("Target");
        var motionSensor = new MotionSensor {Id = id, Name = "Motion Sensor", BatteryLevel = -1, LastValue = false, ExternalIds = { ["deviceId"] = id } };
        var relationship = TwinUtility.GetRelationshipFor(id, "observes", target);
        return (motionSensor, relationship);
    }
}