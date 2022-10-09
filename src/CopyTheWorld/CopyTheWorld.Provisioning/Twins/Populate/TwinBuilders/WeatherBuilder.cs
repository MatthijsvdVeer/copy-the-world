namespace CopyTheWorld.Provisioning.Twins.Populate.TwinBuilders;

using Azure.DigitalTwins.Core;
using Provisioning;
using Shared.TwinModels;
using System;
using System.Data;

internal sealed class WeatherBuilder : ITwinBuilder<Weather>
{
    public (Weather, BasicRelationship) CreateTwinAndRelationship(DataRow dataRow)
    {
        if (dataRow == null)
        {
            throw new ArgumentNullException(nameof(dataRow));
        }

        var target = dataRow.GetStringValue("Target");
        var weather = new Weather
        {
            Id = TwinUtility.CreateIdFromParts("weather", target),
            Name = "Weather",
            Conditions = string.Empty
        };
        
        var relationship = TwinUtility.GetRelationshipFor(weather.Id, "isCapabilityOf", target);

        return (weather, relationship);
    }
}