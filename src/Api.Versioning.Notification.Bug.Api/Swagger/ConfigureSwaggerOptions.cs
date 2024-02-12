using Asp.Versioning.ApiExplorer;
using Asp.Versioning;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

namespace Api.Versioning.Notification.Bug.Api.Swagger;

public class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    /// <summary>
    /// Configure each API discovered for Swagger Documentation
    /// </summary>
    /// <param name="options"></param>
    public void Configure(SwaggerGenOptions options)
    {
        // add swagger document for every API version discovered
        foreach (ApiVersionDescription description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateVersionInfo(description));
        }
    }

    /// <summary>
    /// Configure Swagger Options. Inherited from the Interface
    /// </summary>
    /// <param name="name"></param>
    /// <param name="options"></param>
    public void Configure(string? name, SwaggerGenOptions options)
    {
        Configure(options);
    }

    /// <summary>
    /// Create information about the version of the API
    /// </summary>
    /// <param name="apiVersionDescription"></param>
    /// <returns>Information about the API</returns>
    private static OpenApiInfo CreateVersionInfo(ApiVersionDescription apiVersionDescription)
    {
        OpenApiInfo info = new()
        {
            Title = "Products API",
            Version = apiVersionDescription.ApiVersion.ToString(),
            Description = BuildDescription(apiVersionDescription)
        };

        return info;
    }

    /// <summary>
    /// Enrich description with additional information such as deprecation details.
    /// Source: https://github.com/dotnet/aspnet-api-versioning/wiki/Version-Policies#api-explorer-integration
    /// </summary>
    /// <param name="apiVersionDescription"></param>
    /// <returns>API Description</returns>
    private static string BuildDescription(ApiVersionDescription apiVersionDescription)
    {
        StringBuilder descriptionBuilder = new("API for managing products.");
        if (apiVersionDescription.IsDeprecated)
        {
            _ = descriptionBuilder.Append(" [Deprecated]");
        }

        if (apiVersionDescription.SunsetPolicy is not SunsetPolicy policy)
        {
            return descriptionBuilder.ToString();
        }

        if (policy.Date is DateTimeOffset when)
        {
            _ = descriptionBuilder.Append(" The API will be sunset on ")
                .Append(when.Date.ToShortDateString())
                .Append('.');
        }

        if (!policy.HasLinks)
        {
            return descriptionBuilder.ToString();
        }

        _ = descriptionBuilder.AppendLine();

        for (int i = 0; i < policy.Links.Count; i++)
        {
            LinkHeaderValue link = policy.Links[i];

            // Only add links to HTML pages
            if (link.Type != "text/html")
            {
                continue;
            }

            _ = descriptionBuilder.AppendLine();

            if (link.Title.HasValue)
            {
                _ = descriptionBuilder.Append(link.Title.Value).Append(": ");
            }

            _ = descriptionBuilder.Append(link.LinkTarget.OriginalString);
        }

        return descriptionBuilder.ToString();
    }
}
