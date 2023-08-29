using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore;

public static class WebApplicationBuilderExtensions
{
    public async static Task RunAbpModuleAsync<TModule>(this WebApplicationBuilder builder,  Action<AbpApplicationCreationOptions> optionsAction = null)
        where TModule : IAbpModule
    {
        builder.Host.UseAutofac();
        await builder.AddApplicationAsync<TModule>(optionsAction);
        var app = builder.Build();
        await app.InitializeApplicationAsync();
        await app.RunAsync();
    }
}
