namespace CopyTheWorld.Shared.DeviceMessages
{
    public sealed class RoomMotionSensorMessage
    {
        public bool MotionDetected { get; set; }

        public double BatteryLevel { get; set; }
    }
}
