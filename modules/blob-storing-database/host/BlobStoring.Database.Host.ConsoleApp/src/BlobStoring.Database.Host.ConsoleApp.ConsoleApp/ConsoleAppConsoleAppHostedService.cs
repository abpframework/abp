using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Volo.Abp;

namespace BlobStoring.Database.Host.ConsoleApp.ConsoleApp;

public class ConsoleAppConsoleAppHostedService : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using (var application = AbpApplicationFactory.Create<ConsoleAppConsoleAppModule>(options =>
        {
            options.UseAutofac();
            options.Services.AddLogging(c => c.AddSerilog());
        }))
        {
            application.Initialize();

            var blobService = application.ServiceProvider.GetService<BlobService>();

            await blobService.SaveFile("Test File 2", "Test Content 2");

            application.Shutdown();
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
