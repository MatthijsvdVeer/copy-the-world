﻿namespace CopyTheWorld.Provisioning.Twins.Populate.TwinBuilders;

using Azure.DigitalTwins.Core;
using CopyTheWorld.Provisioning;
using CopyTheWorld.Shared.TwinModels;
using CopyTheWorld.Shared.TwinModels.Components;
using System;
using System.Data;

internal sealed class BuildingBuilder : ITwinBuilder<Building>
{
    public (Building, BasicRelationship) CreateTwinAndRelationship(DataRow dataRow)
    {
        if (dataRow == null)
        {
            throw new ArgumentNullException(nameof(dataRow));
        }

        var building = new Building
        {
            Id = TwinUtility.CreateIdFromParts(dataRow.GetStringValue("ID")),
            Name = dataRow.GetStringValue("Name"),
            Address = new Address
            {
                AddressLine1 = dataRow.GetStringValue("AddressLine1"),
                City = dataRow.GetStringValue("City"),
                Country = dataRow.GetStringValue("Country"),
                PostalCode = dataRow.GetStringValue("PostalCode")
            },
            ExternalIds = {["coords"] = dataRow.GetStringValue("Coordinates")}
        };

        var city = dataRow.GetStringValue("CityTwin");
        var relationship = TwinUtility.GetRelationshipFor(building.Id, "locatedIn", city);

        return (building, relationship);
    }
}