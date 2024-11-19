using CP.Api.Models;
using CP.Api.Options;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CP.Api.Services;

public class BingSearchService(
    ILogger<BingSearchService> logger,
    IOptions<BingOptions> bingOptionsValue,
    HttpClient client)
{
    public async Task<List<SearchModel>> SearchAsync(string query)
    {
        var bingOptions = bingOptionsValue.Value;
        var url = $"{bingOptions.Endpoint}/v7.0/search";
        logger.LogInformation("Searching Bing for {Query} at {Url}", query, url);
        client.BaseAddress = new Uri(url, UriKind.Absolute);
        client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", bingOptions.Key);
        var currentSearchData = $"search?q={Uri.EscapeDataString(query)}";
        logger.LogInformation("Sending search context {SearchContext}", currentSearchData);
        
        var searchResponse = await client.GetAsync(currentSearchData);
        
        var list = new List<SearchModel>();
        logger.LogInformation("Received search response {Response}", searchResponse.StatusCode);
        if (!searchResponse.IsSuccessStatusCode)
        {
            logger.LogError("Failed to get search results from Bing for {Query}", query);
            return list;
        }

        var contentString = await searchResponse.Content.ReadAsStringAsync();
        if (string.IsNullOrEmpty(contentString))
        {
            logger.LogError("Failed to get search results from Bing for {Query}", query);
            return list;
        }

        var webPageResponse = JsonConvert.DeserializeObject<BingResponseObject>(contentString);
        list.AddRange(webPageResponse?.WebPagesResult.WebPageslist.Select(webPageDetail => new SearchModel
        {
            Title = webPageDetail.Name,
            Description = webPageDetail.Description,
            UrlOrigin = webPageDetail.DisplayUrl,
            Image = webPageDetail.ThumbnailUrl
        }) ?? Array.Empty<SearchModel>());
        logger.LogInformation("Received {Count} search results from Bing for {Query}", list.Count, query);
        return list;
    }
}