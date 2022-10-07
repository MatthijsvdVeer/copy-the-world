﻿namespace CopyTheWorld.Provisioning.Populate
{
    using Azure.DigitalTwins.Core;
    using Shared;
    using System.Data;
    using System.Linq;
    using TwinBuilders;

    internal sealed class TwinFactory
    {
        public CreateTwinsAndRelationshipsResult CreateTwinsAndRelationships(DataTable dataTable) =>
            dataTable.TableName switch
            {
                "Buildings" => GetObjects(new BuildingBuilder(), dataTable),
                "Levels" => GetObjects(new LevelBuilder(), dataTable),
                "Rooms" => GetObjects(new RoomBuilder(), dataTable),
                "PhoneBooths" => GetObjects(new PhoneBoothBuilder(), dataTable),
                "Desks" => GetObjects(new FurnitureBuilder(), dataTable),
                "Zones" => GetObjects(new ZoneBuilder(), dataTable),
                "Parking Lots" => GetObjects(new ParkingLotBuilder(), dataTable),
                "Parking spots" => GetObjects(new ParkingSpotBuilder(), dataTable),
                _ => new CreateTwinsAndRelationshipsResult()
            };

        private static CreateTwinsAndRelationshipsResult GetObjects<T>(ITwinBuilder<T> builder, DataTable dataTable) where T : BasicDigitalTwin
        {
            var tuples = dataTable.Rows.Cast<DataRow>().Select(builder.CreateTwinAndRelationship);
            var twins = tuples.Select(tuple => tuple.Item1.SetModelOnTwin()).ToList();
            
            var basicRelationships = tuples.Where(tuple => tuple.Item2 != TwinUtility.EmptyRelation).Select(tuple => tuple.Item2).ToList();
            return new CreateTwinsAndRelationshipsResult(twins, basicRelationships);
        }
    }
}
