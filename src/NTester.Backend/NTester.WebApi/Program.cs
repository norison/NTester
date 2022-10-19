using NLog;
using NLog.Web;
using NTester.WebApi;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Info("NTester Web API initialization.");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    var services = builder.Services;
    var configuration = builder.Configuration;

    services.AddNTesterServices(configuration);

    builder.Build().UseNTester().Run();
}
catch (Exception exception)
{
    logger.Error(exception, "Stopped program because of exception.");
    throw;
}
finally
{
    LogManager.Shutdown();
}