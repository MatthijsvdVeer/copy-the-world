namespace CopyTheWorld.Shared.TwinModels.Components;

using Azure.DigitalTwins.Core;

[Dtmi("dtmi:digitaltwins:rec_3_3:addressing:Address;1")]
public sealed class Address : BasicDigitalTwinComponent
{
    public string? PostalCode { get; set; }

    public string? City { get; set; }

    public string? Country { get; set; }

    public string? AddressLine1 { get; set; }
}