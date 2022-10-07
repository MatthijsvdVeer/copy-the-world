namespace CopyTheWorld.Provisioning.Populate.TwinBuilders
{
    using Azure.DigitalTwins.Core;
    using DigitalTwin.Provisioning;
    using Shared.TwinModels;
    using System.Data;

    internal sealed class ParkingSpotBuilder : ITwinBuilder<ParkingSpot>
    {
        public (ParkingSpot, BasicRelationship) CreateTwinAndRelationship(DataRow dataRow)
        {
            var id = dataRow.GetStringValue("ID");
            var building = dataRow.GetStringValue("Building");
            var parking = dataRow.GetStringValue("Parking");

            var parkingId = TwinUtility.CreateIdFromParts(parking, id);
            var parkingLotId = TwinUtility.CreateIdFromParts(building, parking);


            var parkingLot = new ParkingSpot {Id = parkingId, Name = id};

            var relationship = TwinUtility.GetRelationshipFor(parkingLotId, "includes", parkingLot.Id);

            return (parkingLot, relationship);
        }
    }
}