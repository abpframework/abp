using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MyCompanyName.MyProjectName.Blazor.Server.Client;

namespace MyCompanyName.MyProjectName.Blazor;

public class Program
{
    public async static Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        var application = await builder.AddApplicationAsync<MyProjectNameBlazorClientModule>(options =>
        {
            options.UseAutofac();
        });

        var host = builder.Build();

        await application.InitializeApplicationAsync(host.Services);

        await host.RunAsync();
    }
}
