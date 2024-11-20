using System.Text.Json.Serialization;
using Azure.Search.Documents.Indexes;

namespace CP.Api.Models;

public class KBSearchResult
{
    [SimpleField(IsKey = true, IsFilterable = true)]
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [SearchableField(IsFilterable = true)]
    [JsonPropertyName("title")]
    public string Title { get; set; }
    [SearchableField(IsFilterable = false)]
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("location")]
    [SearchableField(IsFilterable = false)]
    public string Location { get; set; }
    [JsonPropertyName("chunk")]
    [SearchableField(IsFilterable = false)]
    public string Chunk { get; set; }
    [VectorSearchField]
    [JsonPropertyName("chunkVector")]
    public ReadOnlyMemory<float> ChunkVector { get; set; }
}