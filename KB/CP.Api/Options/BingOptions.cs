using System.ComponentModel.DataAnnotations;

namespace CP.Api.Options;

public class BingOptions
{
    [Required(ErrorMessage = "Key must be defined")] 
    public required string Key { get; init; }
    [Required(ErrorMessage = "Endpoint must be defined")]
    public required string Endpoint { get; init; } = "https://api.bing.microsoft.com/";
}