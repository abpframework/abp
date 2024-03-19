using AbpPerfTest.WithAbp;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseAutofac();
await builder.AddApplicationAsync<AppModule>();
var app = builder.Build();
await app.InitializeApplicationAsync();
await app.RunAsync();
