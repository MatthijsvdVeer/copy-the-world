namespace CopyTheWorld.Functions
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    internal sealed class TwinUpdateData
    {
        [JsonPropertyName("modelId")]
        public string ModelId { get; set; }

        [JsonPropertyName("patch")]
        public IEnumerable<TwinUpdatePatch> Patches { get; set; }
    }
}