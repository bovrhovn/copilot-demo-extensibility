using System.ComponentModel.DataAnnotations;

namespace CP.Api.Options;

public class SqlOptions
{
    [Required(ErrorMessage = "SqlOptions:ConnectionString is required")]
    public required string ConnectionString { get; set; }
}