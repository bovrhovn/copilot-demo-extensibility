﻿using CP.Api.Helpers;
using CP.Api.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CP.Api.Authentication;

/// <summary>
///  filter to enable validation if the right service has called the api
/// </summary>
/// <remarks>
///     implemented based on this video https://www.youtube.com/watch?v=GrJJXixjR8M&t=48s
///     by Nick Chapsas 
/// </remarks>
public class ApiKeyAuthFilter(IConfiguration configuration) : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(AuthOptions.ApiKeyHeaderName, out var extractedApiKey))
        {
            context.Result = new UnauthorizedObjectResult("Api Key is missing");
            return;
        }
        var apiKey = configuration
            .GetSection(SettingsNameHelper.AuthOptionsSectionName)
            .Get<AuthOptions>()!
            .ApiKey;
        if (!apiKey.Equals(extractedApiKey))
            context.Result = new UnauthorizedObjectResult("Api Key is not valid");
    }
}