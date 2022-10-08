namespace CopyTheWorld.Functions
{
    using System.Text.Json.Serialization;

    internal sealed class TwinUpdate
    {
        [JsonPropertyName("data")]
        public TwinUpdateData Data { get; set; }
    }
}