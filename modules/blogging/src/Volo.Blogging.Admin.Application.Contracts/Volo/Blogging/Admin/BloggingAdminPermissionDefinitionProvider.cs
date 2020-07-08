using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Blogging.Localization;

namespace Volo.Blogging.Admin
{
    public class BloggingAdminPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var bloggingGroup = context.AddGroup(BloggingAdminPermissions.GroupName, L("Permission:BloggingAdmin"));

            var blogs = bloggingGroup.AddPermission(BloggingAdminPermissions.Blogs.Default, L("Permission:Blogs"));
            blogs.AddChild(BloggingAdminPermissions.Blogs.Management, L("Permission:Management"));
            blogs.AddChild(BloggingAdminPermissions.Blogs.Update, L("Permission:Edit"));
            blogs.AddChild(BloggingAdminPermissions.Blogs.Delete, L("Permission:Delete"));
            blogs.AddChild(BloggingAdminPermissions.Blogs.Create, L("Permission:Create"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<BloggingResource>(name);
        }
    }
}
