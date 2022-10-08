using Azure.Data.Tables;
using Azure;

public sealed class MappingEntity : ITableEntity
{
    public string PartitionKey { get; set; }

    public string RowKey { get; set; }

    public DateTimeOffset? Timestamp { get; set; }

    public ETag ETag { get; set; }

    public string Mapping { get; set; }
}