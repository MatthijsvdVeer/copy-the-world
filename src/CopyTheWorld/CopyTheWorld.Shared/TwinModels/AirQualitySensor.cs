namespace CopyTheWorld.Shared.TwinModels
{
    using Azure.DigitalTwins.Core;

    [Dtmi("dtmi:digitaltwins:rec_3_3:device:AirQualitySensor;1")]
    public abstract class AirQualitySensor : BasicDigitalTwin
    {
        public double LastValue { get; set; }

        public Dictionary<string, string> ExternalIds { get; set; }

        protected AirQualitySensor() => this.ExternalIds = new Dictionary<string, string>();
    }
}