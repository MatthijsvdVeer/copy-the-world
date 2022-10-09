namespace CopyTheWorld.Provisioning.Twins.Populate.TwinBuilders;

using Azure.DigitalTwins.Core;
using CopyTheWorld.Provisioning;
using CopyTheWorld.Shared.TwinModels;
using System.Data;

internal sealed class CountryBuilder : ITwinBuilder<Country>
{
    public (Country, BasicRelationship) CreateTwinAndRelationship(DataRow dataRow)
    {
        var id = dataRow.GetStringValue("ID");
        var name = dataRow.GetStringValue("Name");

        var level = new Country
        {
            Id = id,
            Name = name
        };

        var planet = dataRow.GetStringValue("Planet");
        var relationship = TwinUtility.GetRelationshipFor(id, "locatedOn", planet);

        return (level, relationship);
    }
}