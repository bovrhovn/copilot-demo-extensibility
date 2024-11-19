namespace CP.Api.Models;

using Newtonsoft.Json;

public class BingResponseObject
{
    public string _type { get; set; }
    [JsonProperty("queryContext")]
    public SearchQueryContext QueryContext { get; set; }
    [JsonProperty("webPages")]
    public WebPages WebPagesResult { get; set; }
    [JsonProperty("images")]
    public Images ImagesResult { get; set; }
    [JsonProperty("relatedSearches")]
    public RelatedSearches RelatedSearchResult { get; set; }
    [JsonProperty("videos")]
    public Videos VideosResult { get; set; }
    [JsonProperty("rankingResponse")]
    public RankingResponse RankingResponse { get; set; }
}

public class SearchQueryContext
{
    public string originalQuery { get; set; }
}

public class WebPages
{
    [JsonProperty("webSearchUrl")]
    public string WebSearchUrl { get; set; }
    [JsonProperty("totalEstimatedMatches")]
    public int TotalEstimatedMatches { get; set; }
    [JsonProperty("value")]
    public WebPageDetail[] WebPageslist { get; set; }
}

public class WebPageDetail
{
    public string id { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("url")]
    public string Url { get; set; }
    [JsonProperty("thumbnailUrl")]
    public string ThumbnailUrl { get; set; }
    [JsonProperty("isFamilyFriendly")]
    public bool IsFamilyFriendly { get; set; }
    [JsonProperty("displayUrl")]
    public string DisplayUrl { get; set; }
    [JsonProperty("snippet")]
    public string Description { get; set; }
    [JsonProperty("deepLinks")]
    public DeepLinks[] DeepLinks { get; set; }
    [JsonProperty("dateLastCrawled")]
    public string DateLastCrawled { get; set; }
    [JsonProperty("language")]
    public string Language { get; set; }
    [JsonProperty("isNavigational")]
    public bool IsNavigational { get; set; }
    [JsonProperty("contractualRules")]
    public ContractualRules[] ContractualRules { get; set; }
}

public class DeepLinks
{
    public string name { get; set; }
    public string url { get; set; }
}

public class ContractualRules
{
    public string _type { get; set; }
    public string targetPropertyName { get; set; }
    public int targetPropertyIndex { get; set; }
    public bool mustBeCloseToContent { get; set; }
    public License license { get; set; }
    public string licenseNotice { get; set; }
}

public class License
{
    public string name { get; set; }
    public string url { get; set; }
}

public class Images
{
    public string id { get; set; }
    public string readLink { get; set; }
    public string webSearchUrl { get; set; }
    public bool isFamilyFriendly { get; set; }
    [JsonProperty("value")]
    public ImageDetail[] Details { get; set; }
}

public class ImageDetail
{
    public string webSearchUrl { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("thumbnailUrl")]
    public string ThumbnailUrl { get; set; }
    [JsonProperty("datePublished")] 
    public string DatePublished { get; set; }
    public string contentUrl { get; set; }
    public string hostPageUrl { get; set; }
    public string contentSize { get; set; }
    public string encodingFormat { get; set; }
    public string hostPageDisplayUrl { get; set; }
    public int width { get; set; }
    public int height { get; set; }
    [JsonProperty("thumbnail")]  
    public Thumbnail Sizes { get; set; }
}

public class Thumbnail
{
    public int width { get; set; }
    public int height { get; set; }
}

public class RelatedSearches
{
    public string id { get; set; }
    [JsonProperty("value")]  
    public RelatedSearchDetail[] RelatedSearchDetails { get; set; }
}

public class RelatedSearchDetail
{
    [JsonProperty("text")]     
    public string Text { get; set; }
    [JsonProperty("displayText")]
    public string DisplayText { get; set; }
    public string webSearchUrl { get; set; }
}

public class Videos
{
    public string id { get; set; }
    public string readLink { get; set; }
    public string webSearchUrl { get; set; }
    public bool isFamilyFriendly { get; set; }
    [JsonProperty("value")]
    public VideoDetail[] Details { get; set; }
    public string scenario { get; set; }
}

public class VideoDetail
{
    public string webSearchUrl { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public string thumbnailUrl { get; set; }
    public string datePublished { get; set; }
    public Publisher[] publisher { get; set; }
    public Creator creator { get; set; }
    public bool isAccessibleForFree { get; set; }
    public string contentUrl { get; set; }
    public string hostPageUrl { get; set; }
    public string encodingFormat { get; set; }
    public string hostPageDisplayUrl { get; set; }
    public int width { get; set; }
    public int height { get; set; }
    public string duration { get; set; }
    public string motionThumbnailUrl { get; set; }
    public string embedHtml { get; set; }
    public bool allowHttpsEmbed { get; set; }
    public int viewCount { get; set; }
    [JsonProperty("thumbnail")]
    public VideoThumbnail ThumbnailSizes { get; set; }
    public bool allowMobileEmbed { get; set; }
    public bool isSuperfresh { get; set; }
}

public class Publisher
{
    public string name { get; set; }
}

public class Creator
{
    public string name { get; set; }
}

public class VideoThumbnail
{
    public int width { get; set; }
    public int height { get; set; }
}

public class RankingResponse
{
    public Mainline mainline { get; set; }
}

public class Mainline
{
    public Items[] items { get; set; }
}

public class Items
{
    public string answerType { get; set; }
    public int resultIndex { get; set; }
    [JsonProperty("value")]
    public ItemDetail Details { get; set; }
}

public class ItemDetail
{
    public string id { get; set; }
}
