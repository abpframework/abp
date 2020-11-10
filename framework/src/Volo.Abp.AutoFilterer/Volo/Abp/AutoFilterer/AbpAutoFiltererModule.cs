using AutoFilterer.Swagger;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Modularity;

namespace Volo.Abp.AutoFilterer
{
    public class AbpAutoFiltererModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Configure<SwaggerGenOptions>(cfg => cfg.UseAutoFiltererParameters());
            base.ConfigureServices(context);
        }
    }
}
