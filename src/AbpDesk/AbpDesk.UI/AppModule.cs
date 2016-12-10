using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Volo.Abp.AspNetCore;
using Volo.Abp.AspNetCore.Modularity;
using Volo.Abp.Modularity;

namespace AbpDesk
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
