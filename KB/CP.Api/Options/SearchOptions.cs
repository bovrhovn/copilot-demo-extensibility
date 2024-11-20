using System.ComponentModel.DataAnnotations;

namespace CP.Api.Options;

public class SearchOptions
{
    [Required(ErrorMessage = "Key must be defined")] 
    public required string Key { get; init; }
    [Required(ErrorMessage = "Endpoint must be defined")] 
    public required string Endpoint { get; init; }
    [Required(ErrorMessage = "Index must be defined")] 
    public required string IndexName { get; init; }
    public int RecordSize { get; set; } = 50;
}