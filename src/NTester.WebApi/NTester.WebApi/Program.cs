using NTester.DataAccess.Data.DatabaseInitializer;
using NTester.WebApi;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

services.AddNTesterServices(configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider
        .GetRequiredService<IDatabaseInitializer>()
        .InitializeAsync()
        .Wait();
}

app.MapControllers();
app.Run();