using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.AspNetCore.Mvc.UI.Widgets
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcUiBootstrapModule),
        typeof(AbpAspNetCoreMvcUiBundlingModule)
    )]
    public class AbpAspNetCoreMvcUiWidgetsModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpAspNetCoreMvcUiWidgetsModule).Assembly);
            });

            AutoAddWidgets(context.Services);
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddTransient<DefaultViewComponentHelper>();

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpAspNetCoreMvcUiWidgetsModule>();
            });
        }

        private static void AutoAddWidgets(IServiceCollection services)
        {
            var widgetTypes = new List<Type>();

            services.OnRegistred(context =>
            {
                if (WidgetAttribute.IsWidget(context.ImplementationType))
                {
                    widgetTypes.Add(context.ImplementationType);
                }
            });

            services.Configure<AbpWidgetOptions>(options =>
            {
                foreach (var widgetType in widgetTypes)
                {
                    options.Widgets.Add(new WidgetDefinition(widgetType));
                }
            });
        }
    }
}
