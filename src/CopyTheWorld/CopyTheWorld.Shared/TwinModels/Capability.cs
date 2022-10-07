namespace CopyTheWorld.Shared.TwinModels;

using Azure.DigitalTwins.Core;
using Components;

[Dtmi("dtmi:digitaltwins:rec_3_3:core:Capability;1")]
public abstract class Capability : BasicDigitalTwin
{
    public string? Name { get; set; }

    public Dictionary<string, string> ExternalIds { get; set; }

    public CapabilityPropertySet CategorizationProperties { get; set; }

    protected Capability()
    {
        this.CategorizationProperties = new CapabilityPropertySet();
        this.ExternalIds = new Dictionary<string, string>();
    }
}