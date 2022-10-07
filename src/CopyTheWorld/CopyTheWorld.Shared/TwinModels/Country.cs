namespace CopyTheWorld.Shared.TwinModels;

using Azure.DigitalTwins.Core;

[Dtmi("dtmi:digitaltwins:ctw:Country;1")]
public sealed class Country : BasicDigitalTwin
{
    public string? Name { get; set; }
}