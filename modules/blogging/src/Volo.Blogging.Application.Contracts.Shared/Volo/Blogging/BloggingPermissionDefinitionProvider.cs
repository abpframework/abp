using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Blogging.Localization;

namespace Volo.Blogging
{
    public class BloggingPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var bloggingGroup = context.AddGroup(BloggingPermissions.GroupName, L("Permission:Blogging"));

            var blogs = bloggingGroup.AddPermission(BloggingPermissions.Blogs.Default, L("Permission:Blogs"));
            blogs.AddChild(BloggingPermissions.Blogs.Management, L("Permission:Management"));
            blogs.AddChild(BloggingPermissions.Blogs.Update, L("Permission:Edit"));
            blogs.AddChild(BloggingPermissions.Blogs.Delete, L("Permission:Delete"));
            blogs.AddChild(BloggingPermissions.Blogs.Create, L("Permission:Create"));
            blogs.AddChild(BloggingPermissions.Blogs.ClearCache, L("Permission:ClearCache"));

            var posts = bloggingGroup.AddPermission(BloggingPermissions.Posts.Default, L("Permission:Posts"));
            posts.AddChild(BloggingPermissions.Posts.Update, L("Permission:Edit"));
            posts.AddChild(BloggingPermissions.Posts.Delete, L("Permission:Delete"));
            posts.AddChild(BloggingPermissions.Posts.Create, L("Permission:Create"));

            var tags = bloggingGroup.AddPermission(BloggingPermissions.Tags.Default, L("Permission:Tags"));
            tags.AddChild(BloggingPermissions.Tags.Update, L("Permission:Edit"));
            tags.AddChild(BloggingPermissions.Tags.Delete, L("Permission:Delete"));
            tags.AddChild(BloggingPermissions.Tags.Create, L("Permission:Create"));

            var comments = bloggingGroup.AddPermission(BloggingPermissions.Comments.Default, L("Permission:Comments"));
            comments.AddChild(BloggingPermissions.Comments.Update, L("Permission:Edit"));
            comments.AddChild(BloggingPermissions.Comments.Delete, L("Permission:Delete"));
            comments.AddChild(BloggingPermissions.Comments.Create, L("Permission:Create"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<BloggingResource>(name);
        }
    }
}
