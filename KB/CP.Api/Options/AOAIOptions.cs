using System.ComponentModel.DataAnnotations;

namespace CP.Api.Options;

public class AOAIOptions
{
    [Required(ErrorMessage = "Key must be defined")] 
    public required string Key { get; init; }
    [Required(ErrorMessage = "Endpoint must be defined")] 
    public required string Endpoint { get; init; }
    [Required(ErrorMessage = "Embedding deployment name must be defined")] 
    public required string EmbeddingsDeploymentName { get; init; }
}