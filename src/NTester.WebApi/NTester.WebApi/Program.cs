using NTester.WebApi;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

services.AddNTesterServices(configuration);

builder.Build().UseNTester().Run();