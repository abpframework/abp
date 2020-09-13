using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Client;

namespace MyCompanyName.MyProjectName.Blazor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            var application = builder.AddApplication<MyProjectNameBlazorModule>(options =>
            {
                options.UseAutofac();
            });

            var host = builder.Build();

            application.Initialize(host.Services);

            using (var scope = host.Services.CreateScope())
            {
                await scope.ServiceProvider
                    .GetRequiredService<ICachedApplicationConfigurationClient>()
                    .InitializeAsync();
            }

            await host.RunAsync();
        }
    }
}
