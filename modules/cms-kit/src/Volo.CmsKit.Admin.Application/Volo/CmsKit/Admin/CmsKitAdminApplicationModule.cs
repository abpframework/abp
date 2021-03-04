using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Volo.Abp.AutoMapper;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.Comments;
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
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<CmsKitAdminApplicationModule>();

            ConfigureTagOptions();

            ConfigureCommentOptions();

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
                            createPolicies: new[]
                            {
                                CmsKitAdminPermissions.BlogPosts.Create,
                                CmsKitAdminPermissions.BlogPosts.Update
                            },
                            updatePolicies: new[]
                            {
                                CmsKitAdminPermissions.BlogPosts.Create,
                                CmsKitAdminPermissions.BlogPosts.Update
                            },
                            deletePolicies: new[]
                            {
                                CmsKitAdminPermissions.BlogPosts.Create,
                                CmsKitAdminPermissions.BlogPosts.Update
                            }));
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
                                createPolicies: new[]
                                {
                                    CmsKitAdminPermissions.BlogPosts.Create,
                                    CmsKitAdminPermissions.BlogPosts.Update
                                },
                                deletePolicies: new[]
                                {
                                    CmsKitAdminPermissions.BlogPosts.Create,
                                    CmsKitAdminPermissions.BlogPosts.Update,
                                    CmsKitAdminPermissions.BlogPosts.Delete
                                }));
                    }

                    if (GlobalFeatureManager.Instance.IsEnabled<PagesFeature>())
                    {
                        options.EntityTypes.AddIfNotContains(
                            new MediaDescriptorDefinition(
                                PageConsts.EntityType,
                                createPolicies: new[]
                                {
                                    CmsKitAdminPermissions.Pages.Create,
                                    CmsKitAdminPermissions.Pages.Update
                                },
                                deletePolicies: new[]
                                {
                                    CmsKitAdminPermissions.Pages.Create,
                                    CmsKitAdminPermissions.Pages.Update,
                                    CmsKitAdminPermissions.Pages.Delete
                                }));
                    }
                });
            }
        }

        private void ConfigureCommentOptions()
        {
            if (GlobalFeatureManager.Instance.IsEnabled<CommentsFeature>())
            {
                Configure<CmsKitCommentOptions>(options =>
                {
                    if (GlobalFeatureManager.Instance.IsEnabled<BlogsFeature>())
                    {
                        options.EntityTypes.AddIfNotContains(
                            new CommentEntityTypeDefinition(BlogPostConsts.EntityType));

                    }
                });
            }
        }
    }
}
