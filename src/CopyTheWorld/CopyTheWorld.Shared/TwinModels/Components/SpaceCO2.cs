namespace CopyTheWorld.Shared.TwinModels.Components;

using Azure.DigitalTwins.Core;
using System.Text.Json.Serialization;

[Dtmi("dtmi:digitaltwins:rec_3_3:SpaceCO2;1")]
public sealed class SpaceCo2 : BasicDigitalTwinComponent
{
    [JsonPropertyName("CO2Sensor")]
    public double CO2Sensor { get; set; }
}