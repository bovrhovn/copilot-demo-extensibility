﻿using System.Net.Mime;
using CP.Api.Authentication;
using CP.Api.Helpers;
using CP.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CP.Api.Controllers;

[AllowAnonymous, ApiController, Route(RouteHelper.BingDataRoute),
 Produces(MediaTypeNames.Application.Json)]
public class BingDataController(ILogger<BingDataController> logger, BingSearchService bingSearchService) : ControllerBase
{
    [HttpGet]
    [Route(RouteHelper.HealthRoute)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult IsAlive()
    {
        logger.LogInformation("Called alive Bing endpoint at {DateCalled}", DateTime.UtcNow);
        return new ContentResult
            { StatusCode = 200, Content = $"I am alive at {DateTime.Now} on {Environment.MachineName}" };
    }
    
    [HttpGet]
    [Route(RouteHelper.SearchRoute+ "/{query}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ServiceFilter(typeof(ApiKeyAuthFilter))]
    public async Task<IActionResult> SearchDataAsync(string query)
    {
        logger.LogInformation("Called Bing search endpoint at {DateCalled} with {Query}", DateTime.UtcNow, query);
        var data = await bingSearchService.SearchAsync(query);
        return Ok(data);
    }
}