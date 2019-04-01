﻿using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Acme.BookStore.ConsoleApiClient
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpHttpClientModule),
        typeof(BookStoreApplicationModule)
        )]
    public class ConsoleApiClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(BookStoreApplicationModule).Assembly
            );
        }
    }
}
