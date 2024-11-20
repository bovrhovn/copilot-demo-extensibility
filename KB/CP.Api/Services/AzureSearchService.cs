using System.ClientModel;
using Azure;
using Azure.AI.OpenAI;
using Azure.Core;
using Azure.Core.Pipeline;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using CP.Api.Models;
using CP.Api.Options;
using Microsoft.Extensions.Options;
using OpenAI.Embeddings;
using SearchOptions = CP.Api.Options.SearchOptions;

namespace CP.Api.Services;

public class AzureSearchService(
    ILogger<AzureSearchService> logger,
    IOptions<SearchOptions> searchOptionsValue,
    IOptions<AOAIOptions> aoaiOptionsValue)
{
    public async Task<List<SearchModel>> SearchAsync(string query)
    {
        var searchOptions = searchOptionsValue.Value;
        var endpoint = new Uri(searchOptions.Endpoint, UriKind.Absolute);
        logger.LogInformation("Creating search client for {Endpoint}", endpoint);
        var credential = new AzureKeyCredential(searchOptions.Key);

        var options = new SearchClientOptions
        {
            Retry =
            {
                Mode = RetryMode.Exponential,
                MaxRetries = 3
            }
        };

        var searchClient = new SearchClient(endpoint, searchOptions.IndexName, credential, options);

        var list = new List<SearchModel>();
        try
        {
            var count = await searchClient.GetDocumentCountAsync();
            logger.LogInformation("Found {Count} documents in the index {IndexName}", count.Value,
                searchOptions.IndexName);
            logger.LogInformation("Searching for {Query}", query);
            var currentSearchOptions = new Azure.Search.Documents.SearchOptions
            {
                IncludeTotalCount = true,
                Size = searchOptions.RecordSize,
                SearchFields = { "title", "chunk" },
                Select = { "id","title","location","chunk" }
            };
            var response = await searchClient.SearchAsync<KBSearchResult>(query, currentSearchOptions);
            await foreach (var result in response.Value.GetResultsAsync())
            {
                var currentSearchResult = result.Document;
                if (!string.IsNullOrEmpty(currentSearchResult.Title) ||
                    !string.IsNullOrEmpty(currentSearchResult.Location))
                    list.Add(new SearchModel
                    {
                        Title = currentSearchResult.Title,
                        Description = currentSearchResult.Chunk,
                        UrlOrigin = currentSearchResult.Location,
                        Image = currentSearchResult.Location
                    });
            }
        }
        catch (RequestFailedException ex) when (ex.Status == 404)
        {
            logger.LogError("The index does not exist, check the index name in the search service");
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
        }

        return list;
    }

    public async Task<List<SearchModel>> SearchVectorDataAsync(string query)
    {
        logger.LogInformation("Searching for {Query} in vectorized data", query);
        var searchOptions = searchOptionsValue.Value;
        var endpoint = new Uri(searchOptions.Endpoint, UriKind.Absolute);
        logger.LogInformation("Creating search client for {Endpoint}", endpoint);
        var credential = new AzureKeyCredential(searchOptions.Key);
        var searchClient = new SearchClient(endpoint, searchOptions.IndexName, credential);
        var list = new List<SearchModel>();
        try
        {
            var vectorizedResult = GetEmbeddings(query);

            var response = await searchClient.SearchAsync<KBSearchResult>(
                new Azure.Search.Documents.SearchOptions
                {
                    VectorSearch = new VectorSearchOptions
                    {
                        Queries =
                        {
                            new VectorizedQuery(vectorizedResult)
                                { KNearestNeighborsCount = 3, Fields = { "chunkVector" } }
                        }
                    },
                    Size = searchOptions.RecordSize
                });

            await foreach (var result in response.Value.GetResultsAsync())
            {
                var currentSearchResult = result.Document;
                if (!string.IsNullOrEmpty(currentSearchResult.Title) ||
                    !string.IsNullOrEmpty(currentSearchResult.Location))
                    list.Add(new SearchModel
                    {
                        Title = currentSearchResult.Title,
                        Description = currentSearchResult.Chunk,
                        UrlOrigin = currentSearchResult.Location,
                        Image = currentSearchResult.Location
                    });
            }
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
        }

        return list;
    }

    private ReadOnlyMemory<float> GetEmbeddings(string query)
    {
        var aoaiOptions = aoaiOptionsValue.Value;
        var endpoint = new Uri(aoaiOptions.Endpoint, UriKind.Absolute);
        var credential = new ApiKeyCredential(aoaiOptions.Key);
        logger.LogInformation("Creating OpenAI client for {Endpoint} with {Key}", endpoint, aoaiOptions.Key);
        var openAIClient = new AzureOpenAIClient(endpoint, credential);
        logger.LogInformation("Getting {EmbeddingsDeploymentName} embedding client",
            aoaiOptions.EmbeddingsDeploymentName);
        var embeddingClient = openAIClient.GetEmbeddingClient(aoaiOptions.EmbeddingsDeploymentName);
        OpenAIEmbedding embedding = embeddingClient.GenerateEmbedding(query);
        logger.LogInformation("Generated embedding for {Query}", query);
        return embedding.ToFloats();
    }
}