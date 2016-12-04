using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.AspNetCore;
using Volo.Abp.AspNetCore.Builder;
using Volo.Abp.Modularity;

namespace AspNetCoreDemo
{
    [DependsOn(typeof(AbpAspNetCoreModule))]
    public class AppModule : IAbpModule, IConfigureAspNet
    {
        public void ConfigureServices(IServiceCollection services)
        {
            
        }

        public void Configure(AspNetConfigurationContext context)
        {
            context.LoggerFactory.AddConsole();

            if (context.Environment.IsDevelopment())
            {
                context.App.UseDeveloperExceptionPage();
            }

            context.App.Run(async (ctx) =>
            {
                await ctx.Response.WriteAsync("Hello World 3!");
            });
        }
    }
}
