namespace CopyTheWorld.Shared.DeviceMessages
{
    public sealed class DeskMotionSensorMessage
    {
        public bool Motion { get; set; }

        public DeskMotionSensorMetaData MetaData { get; set; }

        public DeskMotionSensorMessage()
        {
            this.MetaData = new DeskMotionSensorMetaData();
        }
    }
}