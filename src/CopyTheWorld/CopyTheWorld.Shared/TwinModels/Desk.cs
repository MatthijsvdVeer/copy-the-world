namespace CopyTheWorld.Shared.TwinModels;

using Components;

[Dtmi("dtmi:digitaltwins:ctw:Desk;1")]
public sealed class Furniture : Asset
{
    public SpaceOccupancy Occupancy { get; set; }

    public Furniture() => this.Occupancy = new SpaceOccupancy();
}