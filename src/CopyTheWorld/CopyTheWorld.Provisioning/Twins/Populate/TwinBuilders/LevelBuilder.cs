﻿namespace CopyTheWorld.Provisioning.Twins.Populate.TwinBuilders;

using Azure.DigitalTwins.Core;
using CopyTheWorld.Provisioning;
using CopyTheWorld.Shared.TwinModels;
using System.Data;

internal class LevelBuilder : ITwinBuilder<Level>
{
    public (Level, BasicRelationship) CreateTwinAndRelationship(DataRow dataRow)
    {
        var levelId = dataRow.GetStringValue("ID");
        var building = dataRow.GetStringValue("Building");

        var level = new Level
        {
            Id = TwinUtility.CreateIdFromParts(building, levelId),
            Name = $"Floor {levelId}",
            LevelNumber = dataRow.GetIntValue("ID")
        };

        var targetId = TwinUtility.CreateIdFromParts(building);
        var relationship = TwinUtility.GetRelationshipFor(level.Id, "isPartOf", targetId);

        return (level, relationship);
    }
}