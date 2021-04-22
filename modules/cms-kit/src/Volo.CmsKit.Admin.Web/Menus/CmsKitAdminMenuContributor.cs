using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.UI.Navigation;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Localization;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.Web.Menus
{
    public class CmsKitAdminMenuContributor : IMenuContributor
    {
        public async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name == StandardMenus.Main)
            {
                await ConfigureMainMenuAsync(context);
            }
        }

        private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
        {
            await AddCmsMenuAsync(context);
        }

        private async Task AddCmsMenuAsync(MenuConfigurationContext context)
        {
            var l = context.GetLocalizer<CmsKitResource>();

            var cmsMenus = new List<ApplicationMenuItem>();

            cmsMenus.Add(new ApplicationMenuItem(
                    CmsKitAdminMenus.Pages.PagesMenu,
                    l["Pages"].Value,
                    "/Cms/Pages",
                    "fa fa-file-alt")
                .RequireGlobalFeatures(typeof(PagesFeature))
                .RequirePermissions(CmsKitAdminPermissions.Pages.Default));

            cmsMenus.Add(new ApplicationMenuItem(
                    CmsKitAdminMenus.Blogs.BlogsMenu,
                    l["Blogs"],
                    "/Cms/Blogs",
                    "fa fa-blog")
                .RequireGlobalFeatures(typeof(BlogsFeature))
                .RequirePermissions(CmsKitAdminPermissions.Blogs.Default));

            cmsMenus.Add(new ApplicationMenuItem(
                    CmsKitAdminMenus.BlogPosts.BlogPostsMenu,
                    l["BlogPosts"],
                    "/Cms/BlogPosts",
                    "fa fa-file-signature")
                .RequireGlobalFeatures(typeof(BlogsFeature))
                .RequirePermissions(CmsKitAdminPermissions.BlogPosts.Default));

            cmsMenus.Add(new ApplicationMenuItem(
                    CmsKitAdminMenus.Tags.TagsMenu,
                    l["Tags"].Value,
                    "/Cms/Tags",
                    "fa fa-tags")
                .RequireGlobalFeatures(typeof(TagsFeature))
                .RequirePermissions(CmsKitAdminPermissions.Tags.Default));

            cmsMenus.Add(new ApplicationMenuItem(
                    CmsKitAdminMenus.Comments.CommentsMenu,
                    l["Comments"].Value,
                    "/Cms/Comments",
                    "fa fa-comments")
                .RequireGlobalFeatures(typeof(CommentsFeature))
                .RequirePermissions(CmsKitAdminPermissions.Comments.Default));

            if (cmsMenus.Any())
            {
                var cmsMenu = context.Menu.FindMenuItem(CmsKitAdminMenus.GroupName);

                if (cmsMenu == null)
                {
                    cmsMenu = new ApplicationMenuItem(
                        CmsKitAdminMenus.GroupName,
                        l["Cms"],
                        icon: "far fa-newspaper");

                    context.Menu.AddItem(cmsMenu);
                }

                foreach (var menu in cmsMenus)
                {
                    cmsMenu.AddItem(menu);
                }
            }
        }
    }
}
