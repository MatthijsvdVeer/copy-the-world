﻿namespace CopyTheWorld.Provisioning.Twins.Populate.TwinBuilders;

using Azure.DigitalTwins.Core;
using CopyTheWorld.Provisioning;
using CopyTheWorld.Shared.TwinModels;
using System.Data;

internal sealed class CityBuilder : ITwinBuilder<City>
{
    public (City, BasicRelationship) CreateTwinAndRelationship(DataRow dataRow)
    {
        var id = dataRow.GetStringValue("ID");
        var name = dataRow.GetStringValue("Name");
        var population = dataRow.GetIntValue("Population");

        var level = new City
        {
            Id = id,
            Name = name,
            Population = population
        };

        var country = dataRow.GetStringValue("Country");
        var relationship = TwinUtility.GetRelationshipFor(id, "locatedIn", country);

        return (level, relationship);
    }
}