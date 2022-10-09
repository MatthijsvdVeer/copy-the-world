namespace CopyTheWorld.Provisioning.Twins.Populate.TwinBuilders;

using Azure.DigitalTwins.Core;
using CopyTheWorld.Provisioning;
using CopyTheWorld.Shared.TwinModels;
using System.Data;

internal sealed class RoomBuilder : ITwinBuilder<Room>
{
    public (Room, BasicRelationship) CreateTwinAndRelationship(DataRow dataRow)
    {
        var id = dataRow.GetStringValue("ID");
        var building = dataRow.GetStringValue("Building");
        var zone = dataRow.GetStringValue("Zone");
        var level = dataRow.GetStringValue("Level");

        string roomId;
        string relationTargetId;
        if (!string.IsNullOrEmpty(zone))
        {
            roomId = TwinUtility.CreateIdFromParts(building, level, id);
            relationTargetId = TwinUtility.CreateIdFromParts(building, level, zone);
        }
        else
        {
            roomId = TwinUtility.CreateIdFromParts(building, level, id);
            relationTargetId = TwinUtility.CreateIdFromParts(building, level);
        }

        var room = new Room
        {
            Id = roomId, 
            Name = dataRow.GetStringValue("Name")
        };
            
        var relationship = TwinUtility.GetRelationshipFor(room.Id, "isPartOf", relationTargetId);

        return (room, relationship);
    }
}