using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Volo.Abp.AspNetCore.Mvc.Json
{
    public class AbpMvcNewtonsoftJsonOptionsSetup : IConfigureOptions<MvcNewtonsoftJsonOptions>
    {
        protected IServiceProvider ServiceProvider { get; }

        public AbpMvcNewtonsoftJsonOptionsSetup(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public void Configure(MvcNewtonsoftJsonOptions options)
        {
            options.SerializerSettings.ContractResolver = ServiceProvider.GetRequiredService<AbpMvcJsonContractResolver>();
        }
    }
}
