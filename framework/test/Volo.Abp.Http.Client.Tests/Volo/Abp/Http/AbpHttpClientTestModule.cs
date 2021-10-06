using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Conventions;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.Abp.Http.DynamicProxying;
using Volo.Abp.Http.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.TestApp;
using Volo.Abp.TestApp.Application.Dto;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Http
{
    [DependsOn(
        typeof(AbpHttpClientModule),
        typeof(AbpAspNetCoreMvcTestModule)
        )]
    public class AbpHttpClientTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(typeof(TestAppModule).Assembly);
            context.Services.AddHttpClientProxy<IRegularTestController>();

            Configure<AbpRemoteServiceOptions>(options =>
            {
                options.RemoteServices.Default = new RemoteServiceConfiguration("/");
            });


            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpHttpClientTestModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<HttpClientTestResource>("en")
                    .AddVirtualJson("/Volo/Abp/Http/Localization");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("Volo.Abp.Http.DynamicProxying", typeof(HttpClientTestResource));
            });

            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.FormBodyBindingIgnoredTypes.Add(typeof(CreateFileInput));
                options.ConventionalControllers.FormBodyBindingIgnoredTypes.Add(typeof(CreateMultipleFileInput));
            });


            Configure<AbpHttpClientProxyingOptions>(options =>
            {
                options.QueryStringConverts.Add(typeof(List<GetParamsNameValue>), typeof(TestObjectToQueryString));
                options.FormDataConverts.Add(typeof(List<GetParamsNameValue>), typeof(TestObjectToFormData));
            });
        }
    }
}
