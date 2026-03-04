using DistDemoApp;
using Dapr;
using Volo.Abp;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseAutofac();

await builder.AddApplicationAsync<DistDemoAppAspNetCoreDaprModule>();

var app = builder.Build();
await app.InitializeApplicationAsync();
await app.RunAsync("http://localhost:8090");
