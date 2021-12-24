using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Hosting;
using Serilog.Extensions.Logging;
using Volo.Abp;

namespace MyCompanyName.MyProjectName;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private IAbpApplicationWithInternalServiceProvider _abpApplication;

    protected async override void OnStartup(StartupEventArgs e)
    {
        Log.Logger = new LoggerConfiguration()
#if DEBUG
            .MinimumLevel.Debug()
#else
            .MinimumLevel.Information()
#endif
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Async(c => c.File("Logs/logs.txt"))
            .CreateLogger();

        try
        {
            Log.Information("Starting WPF host.");

            _abpApplication =  await AbpApplicationFactory.CreateAsync<MyProjectNameModule>(options =>
            {
                options.UseAutofac();

                // UseSerilog()
                options.Services.AddLogging();
                options.Services.Replace(ServiceDescriptor.Singleton<ILoggerFactory, SerilogLoggerFactory>());
                var implementationInstance = new DiagnosticContext(null);
                options.Services.AddSingleton(implementationInstance);
                options.Services.AddSingleton((IDiagnosticContext) implementationInstance);
            });

            await _abpApplication.InitializeAsync();

            _abpApplication.Services.GetRequiredService<MainWindow>()?.Show();

        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly!");
        }
    }

    protected async override void OnExit(ExitEventArgs e)
    {
        await _abpApplication.ShutdownAsync();
        Log.CloseAndFlush();
    }
}
