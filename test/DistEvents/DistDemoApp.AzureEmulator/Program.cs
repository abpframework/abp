using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Volo.Abp;
using Volo.Abp.Threading;

namespace DistDemoApp;

public class Program
{
    public static void Main(string[] args)
    {
        using var application = AbpApplicationFactory.Create<DistDemoAppAzureEmulatorModule>(options =>
        {
            options.UseAutofac();
            options.Services.AddSerilog((serviceProvider, configuration) =>
            {
            });
            options.Services.AddLogging(c => c.AddSerilog());
        });

        Log.Information("Starting DistDemoApp.AzureEmulator.");

        application.Initialize();

        AsyncHelper.RunSync(() => application
            .ServiceProvider
            .GetRequiredService<IDistEventScenarioRunner>()
            .RunAsync(DistEventScenarioProfile.AzureEmulator()));

        application.Shutdown();
    }
}
