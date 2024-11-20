using System.Net.Mime;
using CP.Api.Authentication;
using CP.Api.Helpers;
using CP.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CP.Api.Controllers;

[AllowAnonymous, ApiController, Route(RouteHelper.DataRoute),
 Produces(MediaTypeNames.Application.Json)]
public class DataController(ILogger<DataController> logger, AzureSearchService searchService) : ControllerBase
{
    [HttpGet]
    [Route(RouteHelper.HealthRoute)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult IsAlive()
    {
        logger.LogInformation("Called alive data endpoint at {DateCalled}", DateTime.UtcNow);
        return new ContentResult
            { StatusCode = 200, Content = $"I am alive at {DateTime.Now} on {Environment.MachineName}" };
    }

    [HttpGet]
    [Route(RouteHelper.SearchRoute+ "/{query}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ServiceFilter(typeof(ApiKeyAuthFilter))]
    public async Task<IActionResult> SearchDataAsync(string query)
    {
        logger.LogInformation("Called search endpoint at {DateCalled} with {Query}", DateTime.UtcNow, query);
        var data = await searchService.SearchAsync(query);
        logger.LogInformation("Returning {Count} search results for {Query}", data.Count, query);
        return Ok(data);
    }
    
    [HttpGet]
    [Route(RouteHelper.SearchVectorRoute+ "/{query}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ServiceFilter(typeof(ApiKeyAuthFilter))]
    public async Task<IActionResult> SearchVectorDataAsync(string query)
    {
        logger.LogInformation("Called search endpoint at {DateCalled} with {Query}", DateTime.UtcNow, query);
        var data = await searchService.SearchVectorDataAsync(query);
        logger.LogInformation("Returning {Count} search results for {Query} based on vector data", data.Count, query);
        return Ok(data);
    }
}