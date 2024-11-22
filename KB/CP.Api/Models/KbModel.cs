namespace CP.Api.Models;

public class KbModel
{
    public int KnowledgeId { get; set; }
    public required string Name { get; set; }
    public string Description { get; set; }
    public Category Category { get; init; }
}