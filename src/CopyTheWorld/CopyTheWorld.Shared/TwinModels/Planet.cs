namespace CopyTheWorld.Shared.TwinModels;

using Azure.DigitalTwins.Core;

[Dtmi("dtmi:digitaltwins:ctw:Planet;1")]
public sealed class Planet : BasicDigitalTwin
{
    public string? Name { get; set; }
}