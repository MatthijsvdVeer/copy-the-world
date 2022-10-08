namespace CopyTheWorld.Provisioning.Twins.Populate.TwinBuilders;

using Azure.DigitalTwins.Core;
using CopyTheWorld.Provisioning;
using CopyTheWorld.Shared.TwinModels;
using System.Data;

internal class PhoneBoothBuilder : ITwinBuilder<PhoneBooth>
{
    public (PhoneBooth, BasicRelationship) CreateTwinAndRelationship(DataRow dataRow)
    {
        var id = dataRow.GetStringValue("ID");
        var building = dataRow.GetStringValue("Building");
        var zone = dataRow.GetStringValue("Zone");
        var level = dataRow.GetStringValue("Level");

        string phoneBoothId;
        string relationTargetId;
        if (!string.IsNullOrEmpty(zone))
        {
            phoneBoothId = TwinUtility.CreateIdFromParts(building, level, zone, id);
            relationTargetId = TwinUtility.CreateIdFromParts(building, level, zone);
        }
        else
        {
            phoneBoothId = TwinUtility.CreateIdFromParts(building, level, id);
            relationTargetId = TwinUtility.CreateIdFromParts(building, level);
        }

        var phoneBooth = new PhoneBooth
        {
            Id = phoneBoothId,
            Name = dataRow.GetStringValue("Name")
        };
            
        var relationship = TwinUtility.GetRelationshipFor(phoneBooth.Id, "isPartOf", relationTargetId);

        return (phoneBooth, relationship);
    }
}