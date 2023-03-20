using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Volo.Abp.AspNetCore.TestBase;

public static class AbpWebHostBuilderExtensions
{
    public static IWebHostBuilder UseAbpTestServer(this IWebHostBuilder builder)
    {
        return builder.ConfigureServices(services =>
        {
            services.AddScoped<IHostLifetime, AbpNoopHostLifetime>();
            services.AddScoped<IServer, TestServer>();
        });
    }
}
