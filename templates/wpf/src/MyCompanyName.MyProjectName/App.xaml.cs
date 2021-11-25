using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Volo.Abp;

namespace MyCompanyName.MyProjectName;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly IHost _host;
    private readonly IAbpApplicationWithExternalServiceProvider _application;

    public App()
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

        _host = CreateHostBuilder();
        _application = _host.Services.GetService<IAbpApplicationWithExternalServiceProvider>();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        try
        {
            Log.Information("Starting WPF host.");
            await _host.StartAsync();
            Initialize(_host.Services);

            _host.Services.GetService<MainWindow>()?.Show();

        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly!");
        }
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        _application.Shutdown();
        await _host.StopAsync();
        _host.Dispose();
        Log.CloseAndFlush();
    }

    private void Initialize(IServiceProvider serviceProvider)
    {
        _application.Initialize(serviceProvider);
    }

    private IHost CreateHostBuilder()
    {
        return Host
            .CreateDefaultBuilder(null)
            .UseAutofac()
            .UseSerilog()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddApplication<MyProjectNameModule>();
            }).Build();
    }
}
