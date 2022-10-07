namespace CopyTheWorld.Shared.TwinModels;

using System.Text.Json.Serialization;
using Components;

[Dtmi("dtmi:digitaltwins:rec_3_3:core:Building;1")]
public sealed class Building : Space
{
    [JsonPropertyName("address")]
    public Address Address { get; set; }

    public Building() => this.Address = new Address();
}