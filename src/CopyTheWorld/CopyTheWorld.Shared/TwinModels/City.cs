namespace CopyTheWorld.Shared.TwinModels;

using Azure.DigitalTwins.Core;

[Dtmi("dtmi:digitaltwins:ctw:City;1")]
public sealed class City : BasicDigitalTwin
{
    public string? Name { get; set; }

    public int Population { get; set; }
}