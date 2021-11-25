
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.Localization;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit.Permissions;

public class CmsKitAdminPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var cmsGroup = context.GetGroupOrNull(CmsKitAdminPermissions.GroupName) ?? context.AddGroup(CmsKitAdminPermissions.GroupName, L("Permission:CmsKit"));

        var contentGroup = cmsGroup.AddPermission(CmsKitAdminPermissions.Comments.Default, L("Permission:Comments"))
            .RequireGlobalFeatures(typeof(CommentsFeature));
        contentGroup.AddChild(CmsKitAdminPermissions.Comments.Delete, L("Permission:Comments.Delete"))
            .RequireGlobalFeatures(typeof(CommentsFeature));

        var tagGroup = cmsGroup.AddPermission(CmsKitAdminPermissions.Tags.Default, L("Permission:TagManagement"))
            .RequireGlobalFeatures(typeof(TagsFeature));
        tagGroup.AddChild(CmsKitAdminPermissions.Tags.Create, L("Permission:TagManagement.Create"))
            .RequireGlobalFeatures(typeof(TagsFeature));
        tagGroup.AddChild(CmsKitAdminPermissions.Tags.Update, L("Permission:TagManagement.Update"))
            .RequireGlobalFeatures(typeof(TagsFeature));
        tagGroup.AddChild(CmsKitAdminPermissions.Tags.Delete, L("Permission:TagManagement.Delete"))
            .RequireGlobalFeatures(typeof(TagsFeature));

        var pageManagement = cmsGroup.AddPermission(CmsKitAdminPermissions.Pages.Default, L("Permission:PageManagement"))
            .RequireGlobalFeatures(typeof(PagesFeature));
        pageManagement.AddChild(CmsKitAdminPermissions.Pages.Create, L("Permission:PageManagement:Create"))
            .RequireGlobalFeatures(typeof(PagesFeature));
        pageManagement.AddChild(CmsKitAdminPermissions.Pages.Update, L("Permission:PageManagement:Update"))
            .RequireGlobalFeatures(typeof(PagesFeature));
        pageManagement.AddChild(CmsKitAdminPermissions.Pages.Delete, L("Permission:PageManagement:Delete"))
            .RequireGlobalFeatures(typeof(PagesFeature));

        var blogManagement = cmsGroup.AddPermission(CmsKitAdminPermissions.Blogs.Default, L("Permission:BlogManagement"))
            .RequireGlobalFeatures(typeof(BlogsFeature));
        blogManagement.AddChild(CmsKitAdminPermissions.Blogs.Create, L("Permission:BlogManagement.Create"))
            .RequireGlobalFeatures(typeof(BlogsFeature));
        blogManagement.AddChild(CmsKitAdminPermissions.Blogs.Update, L("Permission:BlogManagement.Update"))
            .RequireGlobalFeatures(typeof(BlogsFeature));
        blogManagement.AddChild(CmsKitAdminPermissions.Blogs.Delete, L("Permission:BlogManagement.Delete"))
            .RequireGlobalFeatures(typeof(BlogsFeature));
        blogManagement.AddChild(CmsKitAdminPermissions.Blogs.Features, L("Permission:BlogManagement.Features"))
            .RequireGlobalFeatures(typeof(BlogsFeature));

        var blogPostManagement = cmsGroup.AddPermission(CmsKitAdminPermissions.BlogPosts.Default, L("Permission:BlogPostManagement"))
            .RequireGlobalFeatures(typeof(BlogsFeature));
        blogPostManagement.AddChild(CmsKitAdminPermissions.BlogPosts.Create, L("Permission:BlogPostManagement.Create"))
            .RequireGlobalFeatures(typeof(BlogsFeature));
        blogPostManagement.AddChild(CmsKitAdminPermissions.BlogPosts.Update, L("Permission:BlogPostManagement.Update"))
            .RequireGlobalFeatures(typeof(BlogsFeature));
        blogPostManagement.AddChild(CmsKitAdminPermissions.BlogPosts.Delete, L("Permission:BlogPostManagement.Delete"))
            .RequireGlobalFeatures(typeof(BlogsFeature));

        var menuManagement = cmsGroup.AddPermission(CmsKitAdminPermissions.Menus.Default, L("Permission:MenuManagement"))
            .RequireGlobalFeatures(typeof(MenuFeature));

        menuManagement.AddChild(CmsKitAdminPermissions.Menus.Create, L("Permission:MenuManagement.Create"))
            .RequireGlobalFeatures(typeof(MenuFeature));
        menuManagement.AddChild(CmsKitAdminPermissions.Menus.Update, L("Permission:MenuManagement.Update"))
            .RequireGlobalFeatures(typeof(MenuFeature));
        menuManagement.AddChild(CmsKitAdminPermissions.Menus.Delete, L("Permission:MenuManagement.Delete"))
            .RequireGlobalFeatures(typeof(MenuFeature));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<CmsKitResource>(name);
    }
}
