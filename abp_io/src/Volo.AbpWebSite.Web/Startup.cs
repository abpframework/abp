﻿using System;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp;

namespace Volo.AbpWebSite
{
    public class Startup
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddApplication<AbpWebSiteWebModule>(options =>
            {
                options.UseAutofac();
                options.Configuration.UserSecretsAssembly = typeof(AbpWebSiteWebModule).Assembly;
            });

            return services.BuildServiceProviderFromFactory();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            app.InitializeApplication();
        }
    }
}
