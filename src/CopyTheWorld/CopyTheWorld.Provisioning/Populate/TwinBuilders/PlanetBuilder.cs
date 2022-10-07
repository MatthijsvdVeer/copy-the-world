namespace CopyTheWorld.Provisioning.Populate.TwinBuilders
{
    using Azure.DigitalTwins.Core;
    using DigitalTwin.Provisioning;
    using Shared.TwinModels;
    using System.Data;

    internal class PlanetBuilder : ITwinBuilder<Planet>
    {
        public (Planet, BasicRelationship) CreateTwinAndRelationship(DataRow dataRow)
        {
            var id = dataRow.GetStringValue("ID");
            var name = dataRow.GetStringValue("Name");

            var furniture = new Planet
            {
                Id = id,
                Name = name
            };

            return (furniture, TwinUtility.EmptyRelation);
        }
    }
}
