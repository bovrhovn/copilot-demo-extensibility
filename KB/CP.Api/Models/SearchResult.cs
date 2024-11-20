using Azure.Search.Documents.Indexes;

namespace CP.Api.Models;

public class SearchResult
{
    [SimpleField(IsKey = true, IsFilterable = true)]
    public string Id { get; set; }
    [SearchableField(IsFilterable = true)]
    public string Title { get; set; }
    [SearchableField(IsFilterable = false)]
    public string Name { get; set; }
    [SearchableField(IsFilterable = false)]
    public string Location { get; set; }
    [SearchableField(IsFilterable = false)]
    public string Chunk { get; set; }
    [VectorSearchField]
    public ReadOnlyMemory<float> ChunkVector { get; set; }
}