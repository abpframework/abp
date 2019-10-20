using AbpIoLocalization.Account.Localization;
using AbpIoLocalization.Admin.Localization;
using AbpIoLocalization.Base.Localization;
using AbpIoLocalization.Blog.Localization;
using AbpIoLocalization.Commercial.Localization;
using AbpIoLocalization.Docs.Localization;
using AbpIoLocalization.Support.Localization;
using AbpIoLocalization.Www;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Localization.Resources.AbpValidation;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace AbpIoLocalization
{
    [DependsOn(typeof(AbpLocalizationModule))]
    public class AbpIoLocalizationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpIoLocalizationModule>("AbpIoLocalization");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("Volo.AbpIo.Commercial", typeof(AbpIoCommercialResource));
                options.MapCodeNamespace("Volo.AbpIo.Domain", typeof(AbpIoBaseResource));
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<AbpIoBaseResource>("en")
                    .AddBaseTypes(
                        typeof(AbpValidationResource)
                        )
                    .AddVirtualJson("/Base/Localization/Resources");

                options.Resources
                    .Add<AbpIoAccountResource>("en")
                    .AddVirtualJson("/Account/Localization/Resources")
                    .AddBaseTypes(typeof(AbpIoBaseResource));

                options.Resources
                    .Add<AbpIoAdminResource>("en")
                    .AddVirtualJson("/Admin/Localization/Resources")
                    .AddBaseTypes(typeof(AbpIoBaseResource));

                options.Resources
                    .Add<AbpIoBlogResource>("en")
                    .AddVirtualJson("/Blog/Localization/Resources")
                    .AddBaseTypes(typeof(AbpIoBaseResource));

                options.Resources
                    .Add<AbpIoCommercialResource>("en")
                    .AddVirtualJson("/Commercial/Localization/Resources")
                    .AddBaseTypes(typeof(AbpIoBaseResource));

                options.Resources
                    .Add<AbpIoDocsResource>("en")
                    .AddVirtualJson("/Docs/Localization/Resources")
                    .AddBaseTypes(typeof(AbpIoBaseResource));

                options.Resources
                    .Add<AbpIoSupportResource>("en")
                    .AddVirtualJson("/Support/Localization/Resources")
                    .AddBaseTypes(typeof(AbpIoBaseResource));

                options.Resources
                    .Add<AbpIoWwwResource>("en")
                    .AddVirtualJson("/Www/Localization/Resources")
                    .AddBaseTypes(typeof(AbpIoBaseResource));
            });
        }
    }
}