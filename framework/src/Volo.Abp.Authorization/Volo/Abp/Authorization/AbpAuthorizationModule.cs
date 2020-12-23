using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Authorization.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Authorization
{
    [DependsOn(
        typeof(AbpSecurityModule),
        typeof(AbpLocalizationModule),
        typeof(AbpMultiTenancyModule)
        )]
    public class AbpAuthorizationModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.OnRegistred(AuthorizationInterceptorRegistrar.RegisterIfNeeded);
            AutoAddDefinitionProviders(context.Services);
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAuthorizationCore();

            context.Services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();

            context.Services.TryAddTransient<DefaultAuthorizationPolicyProvider>();

            Configure<AbpPermissionOptions>(options =>
            {
                options.ValueProviders.Add<UserPermissionValueProvider>();
                options.ValueProviders.Add<RolePermissionValueProvider>();
                options.ValueProviders.Add<ClientPermissionValueProvider>();
            });

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpAuthorizationResource>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<AbpAuthorizationResource>("en")
                    .AddVirtualJson("/Volo/Abp/Authorization/Localization");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("Volo.Authorization", typeof(AbpAuthorizationResource));
            });
        }

        private static void AutoAddDefinitionProviders(IServiceCollection services)
        {
            var definitionProviders = new List<Type>();

            services.OnRegistred(context =>
            {
                if (typeof(IPermissionDefinitionProvider).IsAssignableFrom(context.ImplementationType))
                {
                    definitionProviders.Add(context.ImplementationType);
                }
            });

            services.Configure<AbpPermissionOptions>(options =>
            {
                options.DefinitionProviders.AddIfNotContains(definitionProviders);
            });
        }
    }
}
