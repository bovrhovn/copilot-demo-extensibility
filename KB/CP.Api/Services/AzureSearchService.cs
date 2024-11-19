namespace CP.Api.Services;

public class AzureSearchService(ILogger<AzureSearchService> logger)
{
    public Task SearchAsync(string query)
    {
        logger.LogInformation("Searching for {Query}", query);
        return Task.CompletedTask;
    }
}