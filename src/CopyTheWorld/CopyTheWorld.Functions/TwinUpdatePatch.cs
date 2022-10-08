namespace CopyTheWorld.Functions
{
    using System.Text.Json.Serialization;

    internal class TwinUpdatePatch
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("path")]
        public string Path { get; set; }

        [JsonPropertyName("op")]
        public string Operation { get; set; }
    }
}