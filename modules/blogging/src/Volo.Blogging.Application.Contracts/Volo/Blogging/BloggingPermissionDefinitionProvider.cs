using System;
using System.Collections.Generic;
using System.Text;
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

            var blogs = bloggingGroup.AddPermission(BloggingPermissions.Blogs.Default, L("Permission:BlogManagement"));
            blogs.AddChild(BloggingPermissions.Blogs.Update, L("Permission:Edit"));
            blogs.AddChild(BloggingPermissions.Blogs.Delete, L("Permission:Delete"));
            blogs.AddChild(BloggingPermissions.Blogs.Create, L("Permission:Create"));

            var posts = bloggingGroup.AddPermission(BloggingPermissions.Posts.Default, L("Permission:PostManagement"));
            posts.AddChild(BloggingPermissions.Posts.Update, L("Permission:Edit"));
            posts.AddChild(BloggingPermissions.Posts.Delete, L("Permission:Delete"));
            posts.AddChild(BloggingPermissions.Posts.Create, L("Permission:Create"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<BloggingResource>(name);
        }
    }
}
