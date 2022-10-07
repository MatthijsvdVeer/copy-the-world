namespace CopyTheWorld.Shared.TwinModels;

using Azure.DigitalTwins.Core;

[Dtmi("dtmi:digitaltwins:xdt:core:ParkingLot;1")]
public sealed class ParkingLot : BasicDigitalTwin
{
    public Dictionary<string, string> ExternalIds { get; set; }

    public ParkingLot() => this.ExternalIds = new Dictionary<string, string>();
}