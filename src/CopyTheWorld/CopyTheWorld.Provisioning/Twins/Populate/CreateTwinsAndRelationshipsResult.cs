namespace CopyTheWorld.Provisioning.Twins.Populate;

using Azure.DigitalTwins.Core;

internal sealed class CreateTwinsAndRelationshipsResult
{
    public IReadOnlyCollection<BasicDigitalTwin> Twins { get; }

    public IReadOnlyCollection<BasicRelationship> Relationships { get; }

    public CreateTwinsAndRelationshipsResult()
        : this(Array.Empty<BasicDigitalTwin>(), Array.Empty<BasicRelationship>())
    {
    }

    public CreateTwinsAndRelationshipsResult(IReadOnlyCollection<BasicDigitalTwin> twins, IReadOnlyCollection<BasicRelationship> relationships)
    {
        this.Twins = twins;
        this.Relationships = relationships;
    }
}