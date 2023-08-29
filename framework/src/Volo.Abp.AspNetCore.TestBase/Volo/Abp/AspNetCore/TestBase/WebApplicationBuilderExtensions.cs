using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.TestBase;

public static class WebApplicationBuilderExtensions
{
    public async static Task RunAbpModuleAsync<TModule>(this WebApplicationBuilder builder,  Action<AbpApplicationCreationOptions>? optionsAction = null)
        where TModule : IAbpModule
    {
        var assemblyName = typeof(TModule).Assembly.GetName()?.Name;
        if (!assemblyName.IsNullOrWhiteSpace())
        {
            // Set the application name as the assembly name of the module will automatically add assembly to the ApplicationParts of MVC application.
            builder.Environment.ApplicationName = assemblyName!;
        }
        builder.Host.UseAutofac();
        await builder.AddApplicationAsync<TModule>(optionsAction);
        var app = builder.Build();
        await app.InitializeApplicationAsync();
        await app.RunAsync();
    }
}
