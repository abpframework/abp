using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Volo.Abp.Domain;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Settings;
using Volo.Abp.Users;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Identity
{
    [DependsOn(typeof(AbpPermissionManagementDomainModule))]
    [DependsOn(typeof(AbpDddDomainModule))]
    [DependsOn(typeof(AbpIdentityDomainSharedModule))]
    [DependsOn(typeof(AbpUsersDomainModule))]
    public class AbpIdentityDomainModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<PermissionManagementOptions>(options =>
            {
                options.ManagementProviders.Add<UserPermissionManagementProvider>();
                options.ManagementProviders.Add<RolePermissionManagementProvider>();
            });

            services.Configure<SettingOptions>(options =>
            {
                options.DefinitionProviders.Add<AbpIdentitySettingDefinitionProvider>();
            });

            services.Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpIdentityDomainModule>();
            });

            services.Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<IdentityResource>()
                    .AddVirtualJson("/Volo/Abp/Identity/Localization/Domain");
            });

            var identityBuilder = services.AddAbpIdentity(options =>
            {
                options.User.RequireUniqueEmail = true;
            });

            services.ExecutePreConfiguredActions(identityBuilder);

            AddAbpIdentityOptionsFactory(services);

            services.AddAssemblyOf<AbpIdentityDomainModule>();
        }

        private static void AddAbpIdentityOptionsFactory(IServiceCollection services)
        {
            services.Replace(ServiceDescriptor.Transient<IOptionsFactory<IdentityOptions>, AbpIdentityOptionsFactory>());
            services.Replace(ServiceDescriptor.Scoped<IOptions<IdentityOptions>, OptionsManager<IdentityOptions>>());
        }
    }
}