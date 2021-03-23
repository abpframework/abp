
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.Localization;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit.Permissions
{
    public class CmsKitAdminPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var cmsGroup = context.GetGroupOrNull(CmsKitAdminPermissions.GroupName) ?? context.AddGroup(CmsKitAdminPermissions.GroupName, L("Permission:CmsKit"));

            if (GlobalFeatureManager.Instance.IsEnabled<CommentsFeature>())
            {
                var contentGroup = cmsGroup.AddPermission(CmsKitAdminPermissions.Comments.Default, L("Permission:Comments"));
                contentGroup.AddChild(CmsKitAdminPermissions.Comments.Delete, L("Permission:Comments.Delete"));
            }
            
            if (GlobalFeatureManager.Instance.IsEnabled<TagsFeature>())
            {
                var tagGroup = cmsGroup.AddPermission(CmsKitAdminPermissions.Tags.Default, L("Permission:TagManagement"));
                tagGroup.AddChild(CmsKitAdminPermissions.Tags.Create, L("Permission:TagManagement.Create"));
                tagGroup.AddChild(CmsKitAdminPermissions.Tags.Update, L("Permission:TagManagement.Update"));
                tagGroup.AddChild(CmsKitAdminPermissions.Tags.Delete, L("Permission:TagManagement.Delete"));
            }

            if (GlobalFeatureManager.Instance.IsEnabled<PagesFeature>())
            {
                var pageManagement = cmsGroup.AddPermission(CmsKitAdminPermissions.Pages.Default, L("Permission:PageManagement"));
                pageManagement.AddChild(CmsKitAdminPermissions.Pages.Create, L("Permission:PageManagement:Create"));
                pageManagement.AddChild(CmsKitAdminPermissions.Pages.Update, L("Permission:PageManagement:Update"));
                pageManagement.AddChild(CmsKitAdminPermissions.Pages.Delete, L("Permission:PageManagement:Delete"));
            }

            if (GlobalFeatureManager.Instance.IsEnabled<BlogsFeature>())
            {
                var blogManagement = cmsGroup.AddPermission(CmsKitAdminPermissions.Blogs.Default, L("Permission:BlogManagement"));
                blogManagement.AddChild(CmsKitAdminPermissions.Blogs.Create, L("Permission:BlogManagement.Create"));
                blogManagement.AddChild(CmsKitAdminPermissions.Blogs.Update, L("Permission:BlogManagement.Update"));
                blogManagement.AddChild(CmsKitAdminPermissions.Blogs.Delete, L("Permission:BlogManagement.Delete"));
                blogManagement.AddChild(CmsKitAdminPermissions.Blogs.Features, L("Permission:BlogManagement.Features"));

                var blogPostManagement = cmsGroup.AddPermission(CmsKitAdminPermissions.BlogPosts.Default, L("Permission:BlogPostManagement"));
                blogPostManagement.AddChild(CmsKitAdminPermissions.BlogPosts.Create, L("Permission:BlogPostManagement.Create"));
                blogPostManagement.AddChild(CmsKitAdminPermissions.BlogPosts.Update, L("Permission:BlogPostManagement.Update"));
                blogPostManagement.AddChild(CmsKitAdminPermissions.BlogPosts.Delete, L("Permission:BlogPostManagement.Delete"));
            }
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<CmsKitResource>(name);
        }
    }
}
