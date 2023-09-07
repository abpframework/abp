using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Volo.BookStore.HttpApi.Client.ConsoleTestApp;

class Program
{
    static async Task Main(string[] args)
    {
        await CreateHostBuilder(args).RunConsoleAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .AddAppSettingsSecretsJson()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<ConsoleTestAppHostedService>();
            });
}
