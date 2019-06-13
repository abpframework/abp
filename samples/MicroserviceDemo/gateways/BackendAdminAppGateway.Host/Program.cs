using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.InProcess;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;

namespace BackendAdminAppGateway.Host
{
    public class Program
    {
        public static int Main(string[] args)
        {
            CurrentDirectoryHelpers.SetCurrentDirectory();

            //TODO: Temporary: it's not good to read appsettings.json here just to configure logging
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.WithProperty("Application", "BackendAdminAppGateway")
                .Enrich.FromLogContext()
                .WriteTo.File("Logs/logs.txt")
                .WriteTo.Elasticsearch(
                    new ElasticsearchSinkOptions(new Uri(configuration["ElasticSearch:Url"]))
                    {
                        AutoRegisterTemplate = true,
                        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6,
                        IndexFormat = "msdemo-log-{0:yyyy.MM}"
                    })
                .CreateLogger();

            try
            {
                Log.Information("Starting BackendAdminAppGateway.Host.");
                BuildWebHostInternal(args).Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "BackendAdminAppGateway.Host terminated unexpectedly!");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHost BuildWebHostInternal(string[] args) =>
            new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIIS()
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseSerilog()
                .Build();
    }
}
