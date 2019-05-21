using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Events;

namespace MyCompanyName.MyProjectName
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
#if DEBUG
                .MinimumLevel.Debug()
#else
                .MinimumLevel.Information()
#endif
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.File("Logs/logs.txt")
                .CreateLogger();

            try
            {
                Log.Information("Starting MyCompanyName.MyProjectName.IdentityServer.");
                BuildWebHostInternal(args).Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "MyCompanyName.MyProjectName.IdentityServer terminated unexpectedly!");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        internal static IWebHost BuildWebHostInternal(string[] args) =>
            new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseSerilog()
                .Build();
    }
}
