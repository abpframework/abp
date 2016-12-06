using Volo.Abp.AspNetCore;
using Volo.Abp.Modularity;
using Volo.Abp.AspNetCore.Modularity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace AspNetCoreDemo
{
    [DependsOn(typeof(AbpAspNetCoreModule))]
    public class AppModule : AbpModule
    {
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            context.GetLoggerFactory().AddConsole();

            if (context.GetEnvironment().IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (ctx) =>
            {
                await ctx.Response.WriteAsync("Hello World 3!");
            });
        }
    }
}
