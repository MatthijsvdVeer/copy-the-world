namespace CopyTheWorld.Shared.TwinModels
{
    using Azure.DigitalTwins.Core;

    [Dtmi("dtmi:digitaltwins:rec_3_3:device:AirQualitySensor;1")]
    public abstract class AirQualitySensor : Capability
    {
        public double LastValue { get; set; }
    }
}