var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAllServices();
var app = builder.Build();

app.SetupMiddlewarePipeline();
await app.RunAsync();
