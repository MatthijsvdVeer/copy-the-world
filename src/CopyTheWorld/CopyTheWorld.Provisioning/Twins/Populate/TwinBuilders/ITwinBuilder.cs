namespace CopyTheWorld.Provisioning.Populate.TwinBuilders;

using Azure.DigitalTwins.Core;
using System.Data;

internal interface ITwinBuilder<T> where T : BasicDigitalTwin
{
    (T, BasicRelationship) CreateTwinAndRelationship(DataRow dataRow);
}