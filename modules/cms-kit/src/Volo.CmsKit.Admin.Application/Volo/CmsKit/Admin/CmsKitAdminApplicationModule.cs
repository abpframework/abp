using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Volo.Abp.AutoMapper;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Localization;
using Volo.CmsKit.MediaDescriptors;
using Volo.CmsKit.Pages;
using Volo.CmsKit.Permissions;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit.Admin
{
    [DependsOn(
        typeof(CmsKitAdminApplicationContractsModule),
        typeof(AbpAutoMapperModule),
        typeof(CmsKitCommonApplicationModule)
        )]
    public class CmsKitAdminApplicationModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            ConfigureTagOptions();
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<CmsKitAdminApplicationModule>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<CmsKitAdminApplicationModule>(validate: true);
            });
        }

        private void ConfigureTagOptions()
        {
            Configure<CmsKitTagOptions>(opts =>
            {
                if (GlobalFeatureManager.Instance.IsEnabled<BlogsFeature>())
                {
                    opts.EntityTypes.AddIfNotContains(
                        new TagEntityTypeDefiniton(
                            BlogPostConsts.EntityType,
                            LocalizableString.Create<CmsKitResource>("BlogPost"),
                            CmsKitAdminPermissions.BlogPosts.Update,
                            CmsKitAdminPermissions.BlogPosts.Update,
                            CmsKitAdminPermissions.BlogPosts.Update));
                }
            });

            if (GlobalFeatureManager.Instance.IsEnabled<MediaFeature>())
            {
                Configure<CmsKitMediaOptions>(options =>
                {
                    if (GlobalFeatureManager.Instance.IsEnabled<BlogsFeature>())
                    {
                        options.EntityTypes.AddIfNotContains(
                            new MediaDescriptorDefinition(
                                BlogPostConsts.EntityType, 
                                createPolicy: CmsKitAdminPermissions.BlogPosts.Update,
                                deletePolicy: CmsKitAdminPermissions.BlogPosts.Delete));
                    }

                    if (GlobalFeatureManager.Instance.IsEnabled<PagesFeature>())
                    {
                        options.EntityTypes.AddIfNotContains(
                            new MediaDescriptorDefinition(
                                PageConsts.EntityType,
                                createPolicy: CmsKitAdminPermissions.Pages.Update,
                                deletePolicy: CmsKitAdminPermissions.Pages.Delete));
                    }
                });
            }
        }
    }
}
