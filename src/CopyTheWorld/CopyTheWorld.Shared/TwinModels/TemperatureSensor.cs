namespace CopyTheWorld.Shared.TwinModels;

using Azure.DigitalTwins.Core;

[Dtmi("dtmi:digitaltwins:rec_3_3:device:TemperatureSensor;1")]
public sealed class TemperatureSensor : Capability
{
    public double LastValue { get; set; }
}