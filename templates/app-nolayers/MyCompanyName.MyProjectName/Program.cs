using Serilog;
using Serilog.Events;

namespace MyCompanyName.MyProjectName;

public class Program
{
    public async static Task<int> Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
#if DEBUG
        .MinimumLevel.Debug()
#else
        .MinimumLevel.Information()
#endif
        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
        .Enrich.FromLogContext()
        .WriteTo.Async(c => c.File("Logs/logs.txt"))
#if DEBUG
        .WriteTo.Async(c => c.Console())
#endif
        .CreateLogger();

        try
        {
            Log.Information("Starting MyCompanyName.MyProjectName.");
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.AddAppSettingsSecretsJson()
                .UseAutofac()
                .UseSerilog();
            await builder.AddApplicationAsync<MyProjectNameModule>();
            var app = builder.Build();
            await app.InitializeApplicationAsync();
            await app.RunAsync();
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "MyCompanyName.MyProjectName terminated unexpectedly!");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
