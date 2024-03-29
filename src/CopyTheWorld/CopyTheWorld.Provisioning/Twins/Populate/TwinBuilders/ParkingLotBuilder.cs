﻿namespace CopyTheWorld.Provisioning.Twins.Populate.TwinBuilders;

using Azure.DigitalTwins.Core;
using CopyTheWorld.Provisioning;
using CopyTheWorld.Shared.TwinModels;
using System.Data;

internal sealed class ParkingLotBuilder : ITwinBuilder<ParkingLot>
{
    public (ParkingLot, BasicRelationship) CreateTwinAndRelationship(DataRow dataRow)
    {
        var levelId = dataRow.GetStringValue("ID");
        var building = dataRow.GetStringValue("Building");

        var level = new ParkingLot()
        {
            Id = TwinUtility.CreateIdFromParts(building, levelId)
        };

        var targetId = TwinUtility.CreateIdFromParts(building);
        var relationship = TwinUtility.GetRelationshipFor(level.Id, "serves", targetId);

        return (level, relationship);
    }
}