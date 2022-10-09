namespace CopyTheWorld.Provisioning.Twins.Populate.TwinBuilders;

using Azure.DigitalTwins.Core;
using CopyTheWorld.Provisioning;
using CopyTheWorld.Shared.TwinModels;
using System.Data;

internal sealed class ZoneBuilder : ITwinBuilder<Zone>
{
    public (Zone, BasicRelationship) CreateTwinAndRelationship(DataRow dataRow)
    {
        var id = dataRow.GetStringValue("ID");
        var building = dataRow.GetStringValue("Building");
        var level = dataRow.GetStringValue("Level");
        var zone = new Zone
        {
            Id = TwinUtility.CreateIdFromParts(building, level, id), 
            Name = $"Zone {dataRow.GetStringValue("Name")}"
        };

        var levelId = TwinUtility.CreateIdFromParts(building, level);
        var relationship = TwinUtility.GetRelationshipFor(zone.Id, "isPartOf", levelId);

        return (zone, relationship);
    }
}