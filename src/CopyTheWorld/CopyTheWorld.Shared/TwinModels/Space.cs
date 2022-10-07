namespace CopyTheWorld.Shared.TwinModels;

using System.Text.Json.Serialization;
using Azure.DigitalTwins.Core;
using Components;

[Dtmi("dtmi:digitaltwins:rec_3_3:core:Space;1")]
public abstract class Space : BasicDigitalTwin
{
    public string? Name { get; set; }

    public Dictionary<string, string> ExternalIds { get; set; }

    public SpaceArea Area { get; set; }

    public SpaceCapacity Capacity { get; set; }

    public SpaceOccupancy Occupancy { get; set; }

    public SpaceTemperature Temperature { get; set; }

    public SpaceHumidity Humidity { get; set; }

    [JsonPropertyName("CO2")]
    public SpaceCo2 Co2 { get; set; }

    protected Space()
    {
        this.ExternalIds = new Dictionary<string, string>();
        this.Area = new SpaceArea();
        this.Capacity = new SpaceCapacity();
        this.Occupancy = new SpaceOccupancy();
        this.Temperature = new SpaceTemperature();
        this.Humidity = new SpaceHumidity();
        this.Co2 = new SpaceCo2();
    }
}