using System;
using Volo.Abp.ObjectExtending.Modularity;

namespace Volo.Abp.ObjectExtending;

public class CmsKitModuleExtensionConfiguration : ModuleExtensionConfiguration
{
    public CmsKitModuleExtensionConfiguration ConfigureBlog(
        Action<EntityExtensionConfiguration> configureAction)
    {
        return this.ConfigureEntity(
            CmsKitModuleExtensionConsts.EntityNames.Blog,
            configureAction
        );
    }

    public CmsKitModuleExtensionConfiguration ConfigureBlogPost(
        Action<EntityExtensionConfiguration> configureAction)
    {
        return this.ConfigureEntity(
            CmsKitModuleExtensionConsts.EntityNames.BlogPost,
            configureAction
        );
    }

    public CmsKitModuleExtensionConfiguration ConfigureBlogFeature(
        Action<EntityExtensionConfiguration> configureAction)
    {
        return this.ConfigureEntity(
            CmsKitModuleExtensionConsts.EntityNames.BlogFeature,
            configureAction
        );
    }

    public CmsKitModuleExtensionConfiguration ConfigureMediaDescriptor(
        Action<EntityExtensionConfiguration> configureAction)
    {
        return this.ConfigureEntity(
            CmsKitModuleExtensionConsts.EntityNames.MediaDescriptor,
            configureAction
        );
    }

    public CmsKitModuleExtensionConfiguration ConfigurePage(
        Action<EntityExtensionConfiguration> configureAction)
    {
        return this.ConfigureEntity(
            CmsKitModuleExtensionConsts.EntityNames.Page,
            configureAction
        );
    }

    public CmsKitModuleExtensionConfiguration ConfigureTag(
        Action<EntityExtensionConfiguration> configureAction)
    {
        return this.ConfigureEntity(
            CmsKitModuleExtensionConsts.EntityNames.Tag,
            configureAction
        );
    }

    public CmsKitModuleExtensionConfiguration ConfigureComment(
        Action<EntityExtensionConfiguration> configureAction)
    {
        return this.ConfigureEntity(
            CmsKitModuleExtensionConsts.EntityNames.Comment,
            configureAction
        );
    }

    public CmsKitModuleExtensionConfiguration ConfigureMenuItem(
        Action<EntityExtensionConfiguration> configureAction)
    {
        return this.ConfigureEntity(
            CmsKitModuleExtensionConsts.EntityNames.MenuItem,
            configureAction
        );
    }

    public CmsKitModuleExtensionConfiguration ConfigureCmsUser(
        Action<EntityExtensionConfiguration> configureAction)
    {
        return this.ConfigureEntity(
            CmsKitModuleExtensionConsts.EntityNames.CmsUser,
            configureAction
        );
    }

    public CmsKitModuleExtensionConfiguration ConfigureGlobalResource(
        Action<EntityExtensionConfiguration> configureAction)
    {
        return this.ConfigureEntity(
            CmsKitModuleExtensionConsts.EntityNames.GlobalResource,
            configureAction
        );
    }
}