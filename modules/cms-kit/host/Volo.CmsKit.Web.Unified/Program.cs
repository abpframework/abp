using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace Volo.CmsKit;

public class Program
{
    public static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("Logs/logs.txt")
            .CreateLogger();

        try
        {
            Log.Information("Starting web host.");

            var builder = WebApplication.CreateBuilder(args);

            builder.Host
#if MongoDB
                .ConfigureAppConfiguration(options =>
                {
                    options.AddJsonFile("appsettings.MongoDB.json");
                })
#endif
                .UseAutofac()
                .UseSerilog();

            await builder.AddApplicationAsync<CmsKitWebUnifiedModule>();

            var app = builder.Build();

            await app.InitializeApplicationAsync();

            await app.RunAsync();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly!");
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }
}