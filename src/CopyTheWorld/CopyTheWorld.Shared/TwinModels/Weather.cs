namespace CopyTheWorld.Shared.TwinModels;

[Dtmi("dtmi:digitaltwins:ctw:Weather;1")]
public sealed class Weather : Capability
{
    public string? Conditions { get; set; }

    public double Temperature { get; set; }
}