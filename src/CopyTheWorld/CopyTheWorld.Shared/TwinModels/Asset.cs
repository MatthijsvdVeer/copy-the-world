namespace CopyTheWorld.Shared.TwinModels;

using Azure.DigitalTwins.Core;

[Dtmi("dtmi:digitaltwins:rec_3_3:core:Asset;1")]
public class Asset : BasicDigitalTwin
{
    public string? Name { get; set; }

    public Dictionary<string, string> ExternalIds { get; set; }

    public Asset() => this.ExternalIds = new Dictionary<string, string>();
}