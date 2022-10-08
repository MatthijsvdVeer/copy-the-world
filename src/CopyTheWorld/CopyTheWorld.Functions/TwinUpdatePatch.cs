namespace CopyTheWorld.Functions
{
    using System.Text.Json.Serialization;

    internal class TwinUpdatePatch
    {
        [JsonPropertyName("value")]
        public object Value { get; set; }

        [JsonPropertyName("path")]
        public string Path { get; set; }

        [JsonPropertyName("op")]
        public string Operation { get; set; }
    }
}