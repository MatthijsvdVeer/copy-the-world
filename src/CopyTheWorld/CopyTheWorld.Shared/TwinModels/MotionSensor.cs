namespace CopyTheWorld.Shared.TwinModels;

[Dtmi("dtmi:digitaltwins:ctw:MotionSensor;1")]
public sealed class MotionSensor : Capability
{
    public double BatteryLevel { get; set; }

    public bool LastValue { get; set; }
}