using System.Data;
using System.Net.Mime;
using CP.Api.Authentication;
using CP.Api.Helpers;
using CP.Api.Models;
using CP.Api.Options;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace CP.Api.Controllers;

[AllowAnonymous, ApiController, Route(RouteHelper.SqlRoute),
 Produces(MediaTypeNames.Application.Json)]
public class SqlController(
    ILogger<SqlController> logger,
    IOptions<SqlOptions> sqlOptionsValue) : ControllerBase
{
    private readonly SqlOptions sqlOptions = sqlOptionsValue.Value;

    [HttpGet]
    [Route(RouteHelper.GetCategoriesRoute)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ServiceFilter(typeof(ApiKeyAuthFilter))]
    public async Task<IActionResult> GetCategoriesAsync()
    {
        logger.LogInformation("Called categories endpoint at {DateCalled}", DateTime.UtcNow);
        await using var connection = new SqlConnection(sqlOptions.ConnectionString);
        var query = "SELECT P.CategoryId,P.Name FROM Categories P";
        var categories = await connection.QueryAsync<Category>(query);
        logger.LogInformation("Returning {Count} categories", categories.Count());
        return Ok(categories);
    }

    [HttpGet]
    [Route(RouteHelper.GetKbRoute)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ServiceFilter(typeof(ApiKeyAuthFilter))]
    public async Task<IActionResult> GetKnowledgeBaseAsync()
    {
        logger.LogInformation("Called knowledge base endpoint at {DateCalled}", DateTime.UtcNow);
        await using var connection = new SqlConnection(sqlOptions.ConnectionString);
        var query = "SELECT P.KnowledgeId,P.Name, P.Description, C.CategoryId, C.Name FROM Knowledge P " +
                    "JOIN Categories C ON P.CategoryId = C.CategoryId";
        var kbModels = await connection.QueryAsync<KbModel>(query);
        logger.LogInformation("Returning {Count} knowledge data", kbModels.Count());
        return Ok(kbModels);
    }

    [HttpGet]
    [Route(RouteHelper.SearchRoute + "/{query}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ServiceFilter(typeof(ApiKeyAuthFilter))]
    public async Task<IActionResult> SearchDataAsync(string query)
    {
        logger.LogInformation("Called search endpoint at {DateCalled} with {Query}", DateTime.UtcNow, query);
        await using var connection = new SqlConnection(sqlOptions.ConnectionString);
        var currentQuery = "SELECT P.KnowledgeId,P.Name, P.Description, C.CategoryId, C.Name FROM Knowledge P " +
                           "JOIN Categories C ON P.CategoryId = C.CategoryId ";
        if (!string.IsNullOrEmpty(query))
            currentQuery +=
                $"WHERE P.Name LIKE '%{query}%' OR P.Description LIKE '%{query}%' OR C.Name LIKE '%{query}%'";
        var kbModels = await connection.QueryAsync<KbModel>(currentQuery);
        logger.LogInformation("Returning {Count} search results for {Query}", kbModels.Count(), query);
        return Ok(kbModels);
    }

    [HttpGet]
    [Route(RouteHelper.HealthRoute)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> IsAlive()
    {
        logger.LogInformation("Called alive data endpoint at {DateCalled}", DateTime.UtcNow);
        //check sql connection health - return 200 if healthy
        await using var sqlConnection = new SqlConnection(sqlOptions.ConnectionString);
        try
        {
            if (sqlConnection.State == ConnectionState.Closed)
                await sqlConnection.OpenAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return new ContentResult
            {
                StatusCode = 400, Content = "SqlConnection failed"
            };
        }
        finally
        {
            if (sqlConnection.State == ConnectionState.Open)
                await sqlConnection.CloseAsync();
        }

        return new ContentResult
            { StatusCode = 200, Content = $"I am alive at {DateTime.Now} on {Environment.MachineName}" };
    }
}