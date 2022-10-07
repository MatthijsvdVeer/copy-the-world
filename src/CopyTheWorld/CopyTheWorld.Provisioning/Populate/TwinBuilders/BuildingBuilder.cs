namespace CopyTheWorld.Provisioning.Populate.TwinBuilders
{
    using Azure.DigitalTwins.Core;
    using DigitalTwin.Provisioning;
    using Shared.TwinModels;
    using Shared.TwinModels.Components;
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
                }
            };

            return (building, TwinUtility.EmptyRelation);
        }
    }
}
