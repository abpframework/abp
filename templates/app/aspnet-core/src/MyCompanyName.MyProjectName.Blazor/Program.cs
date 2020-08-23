using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;

namespace MyCompanyName.MyProjectName.Blazor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            //TODO: Should be done in the ABP framework!
            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
            builder.Services.AddSingleton(builder);

            builder.Services.AddApplication<MyProjectNameBlazorModule>(opts =>
            {
                opts.UseAutofac();
            });

            builder.ConfigureContainer(builder.Services.GetSingletonInstance<IServiceProviderFactory<ContainerBuilder>>());
            
            await builder.Build().RunAsync();
        }
    }
}
