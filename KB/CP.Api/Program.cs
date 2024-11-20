using System.Net;
using System.Text.Json.Serialization;
using CP.Api.Authentication;
using CP.Api.Helpers;
using CP.Api.Options;
using CP.Api.Services;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOptions<AuthOptions>()
    .Bind(builder.Configuration.GetSection(SettingsNameHelper.AuthOptionsSectionName));
builder.Services.AddOptions<BingOptions>()
    .Bind(builder.Configuration.GetSection(SettingsNameHelper.BingOptionsSectionName));
builder.Services.AddOptions<SearchOptions>()
    .Bind(builder.Configuration.GetSection(SettingsNameHelper.SearchOptionsSectionName));
builder.Services.AddOptions<AOAIOptions>()
    .Bind(builder.Configuration.GetSection(SettingsNameHelper.AzureOpenAiOptionsSectionName));
builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);;
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(conf =>
{
    conf.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "Api key to access the  api's",
        Type = SecuritySchemeType.ApiKey,
        Name = AuthOptions.ApiKeyHeaderName,
        In = ParameterLocation.Header,
        Scheme = "ApiKeyScheme"
    });
    var scheme = new OpenApiSecurityScheme
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "ApiKey"
        },
        In = ParameterLocation.Header
    };
    var requirement = new OpenApiSecurityRequirement
    {
        { scheme, new List<string>() }
    };
    conf.AddSecurityRequirement(requirement);
});

builder.Services.AddScoped<ApiKeyAuthFilter>();
builder.Services.AddHealthChecks();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(new CorsPolicyBuilder()
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin()
        .Build());
});

builder.Services.AddScoped<AzureSearchService>();
builder.Services.AddHttpClient<BingSearchService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseCors();
app.MapControllers();
app.UseExceptionHandler(options =>
{
    options.Run(async context =>
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Response.ContentType = "application/json";
        var exception = context.Features.Get<IExceptionHandlerFeature>();
        if (exception != null)
        {
            var message = $"{exception.Error.Message}";
            await context.Response.WriteAsync(message).ConfigureAwait(false);
        }
    });
});
app.MapHealthChecks($"/{RouteHelper.HealthRoute}", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
}).AllowAnonymous();
app.Run();