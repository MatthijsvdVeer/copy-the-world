namespace CopyTheWorld.Shared.TwinModels;

using Azure.DigitalTwins.Core;

[Dtmi("dtmi:digitaltwins:ctw:MotionSensor;1")]
public sealed class MotionSensor : BasicDigitalTwin
{
    public string? Name { get; set; }

    public double BatteryLevel { get; set; }

    public bool LastValue { get; set; }

    public Dictionary<string, string> ExternalIds { get; set; }

    public MotionSensor() => this.ExternalIds = new Dictionary<string, string>();
}