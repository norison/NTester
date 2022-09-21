using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using NTester.WebApi.Middlewares.CustomExceptionHandler;

namespace NTester.WebApi;

/// <summary>
/// Extensions of the web application.
/// </summary>
[ExcludeFromCodeCoverage]
public static class WebApplicationExtensions
{
    /// <summary>
    /// Add required middlewares for the application.
    /// </summary>
    /// <param name="app">Web application.</param>
    /// <returns>Web application.</returns>
    public static WebApplication UseNTester(this WebApplication app)
    {
        if (app.Environment.IsProduction())
        {
            app.UseHsts();
        }

        ConfigureSwagger(app);

        app.UseCors(options =>
        {
            options
                .WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .WithExposedHeaders("WWW-Authenticate");
        });

        app.UseHttpsRedirection();
        app.UseCustomExceptionHandler();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseApiVersioning();
        app.MapControllers();

        return app;
    }

    private static void ConfigureSwagger(WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            var versionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var versionDescription in versionProvider.ApiVersionDescriptions)
            {
                var groupName = versionDescription.GroupName;
                options.SwaggerEndpoint($"swagger/{groupName}/swagger.json", groupName);
            }

            options.RoutePrefix = string.Empty;
        });
    }
}