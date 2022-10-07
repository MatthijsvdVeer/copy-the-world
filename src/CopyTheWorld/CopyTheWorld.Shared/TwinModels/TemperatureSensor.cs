namespace CopyTheWorld.Shared.TwinModels;

using Azure.DigitalTwins.Core;

[Dtmi("dtmi:digitaltwins:rec_3_3:device:TemperatureSensor;1")]
public sealed class TemperatureSensor : BasicDigitalTwin
{
    public double LastValue { get; set; }

    public Dictionary<string, string> ExternalIds { get; set; }
}