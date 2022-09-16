using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NTester.WebApi.Swagger;

/// <summary>
/// Configures the options for swagger.
/// </summary>
public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider;

    /// <summary>
    /// Creates an instance of the configure swagger options.
    /// </summary>
    /// <param name="apiVersionDescriptionProvider">Provider of the api version description.</param>
    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider apiVersionDescriptionProvider)
    {
        _apiVersionDescriptionProvider = apiVersionDescriptionProvider;
    }

    /// <inheritdoc cref="IConfigureOptions{SwaggerGenOptions}.Configure"/>
    public void Configure(SwaggerGenOptions options)
    {
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "NTester.WebApi.xml"));
        options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            In = ParameterLocation.Header,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            BearerFormat = "JWT",
            Name = "Authorization",
            Description = "Authorization token",
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = JwtBearerDefaults.AuthenticationScheme
                    }
                },
                Array.Empty<string>()
            }
        });

        foreach (var versionDescription in _apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            var apiVersion = versionDescription.ApiVersion.ToString();
            options.SwaggerDoc(versionDescription.GroupName, new OpenApiInfo
            {
                Title = $"NTester Web API {apiVersion}",
                Description = "Allows a user to create own tests and complete the tests from another users.",
                Version = apiVersion,
                Contact = new OpenApiContact
                {
                    Name = "Ihor Matiev",
                    Email = "ihor.matiev@gmail.com"
                },
            });
        }
    }
}