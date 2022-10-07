using Azure.DigitalTwins.Core;

public static class TwinUtility
{
    public static readonly BasicRelationship EmptyRelation = new();

    public static string CreateIdFromParts(params string[] values) => string.Join('-', values).ToLowerInvariant();

    public static BasicRelationship GetRelationshipFor(string sourceId, string relationName, string targetId)
    {
        var id = string.Join('_', sourceId, relationName, targetId).ToLowerInvariant();
        return new BasicRelationship {Id = id, SourceId = sourceId, Name = relationName, TargetId = targetId};
    }
}