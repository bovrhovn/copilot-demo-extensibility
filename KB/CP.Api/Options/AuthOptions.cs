﻿using System.ComponentModel.DataAnnotations;

namespace CP.Api.Options;

public class AuthOptions
{
    public const string ApiKeyHeaderName = "X-Api-Key";
    [Required(ErrorMessage = "Api key must be defined")] 
    public required string ApiKey { get; init; }
    public required string BaseUrl { get; init; }
}