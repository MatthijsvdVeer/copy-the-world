namespace CopyTheWorld.Provisioning.Populate.TwinBuilders;

using Azure.DigitalTwins.Core;
using CopyTheWorld.Provisioning;
using Shared.TwinModels;
using System.Data;

internal class FurnitureBuilder : ITwinBuilder<Furniture>
{
    public (Furniture, BasicRelationship) CreateTwinAndRelationship(DataRow dataRow)
    {
        var id = dataRow.GetStringValue("ID");
        var building = dataRow.GetStringValue("Building");
        var zone = dataRow.GetStringValue("Zone");
        var level = dataRow.GetStringValue("Level");

        string deskId;
        string relationTargetId;
        if (!string.IsNullOrEmpty(zone))
        {
            deskId = TwinUtility.CreateIdFromParts(building, level, zone, id);
            relationTargetId = TwinUtility.CreateIdFromParts(building, level, zone);
        }
        else
        {
            deskId = TwinUtility.CreateIdFromParts(building, level, id);
            relationTargetId = TwinUtility.CreateIdFromParts(building, level);
        }

        var furniture = new Furniture
        {
            Id = deskId,
            Name = id
        };
            
        var relationship = TwinUtility.GetRelationshipFor(furniture.Id, "locatedIn", relationTargetId);

        return (furniture, relationship);
    }
}