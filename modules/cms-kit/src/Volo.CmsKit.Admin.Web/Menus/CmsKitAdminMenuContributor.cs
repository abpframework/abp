using System.Threading.Tasks;
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

            var cmsMenu = context.Menu.FindMenuItem(CmsKitAdminMenus.GroupName);

            cmsMenu ??= new ApplicationMenuItem(
                CmsKitAdminMenus.GroupName,
                l["Cms"],
                icon: "far fa-newspaper");

            if (GlobalFeatureManager.Instance.IsEnabled<CommentsFeature>())
            {
                if (await context.IsGrantedAsync(CmsKitAdminPermissions.Comments.Default))
                {
                    cmsMenu.AddItem(new ApplicationMenuItem(
                            CmsKitAdminMenus.Comments.CommentsMenu,
                            l["Comments"].Value,
                            "/Cms/Comments"
                        )
                    );
                }
            }

            if (GlobalFeatureManager.Instance.IsEnabled<TagsFeature>())
            {
                if (await context.IsGrantedAsync(CmsKitAdminPermissions.Tags.Default))
                {
                    cmsMenu.AddItem(new ApplicationMenuItem(
                            CmsKitAdminMenus.Tags.TagsMenu,
                            l["Tags"].Value,
                            "/Cms/Tags"));
                }
            }

            if (GlobalFeatureManager.Instance.IsEnabled<PagesFeature>())
            {
                if (await context.IsGrantedAsync(CmsKitAdminPermissions.Pages.Default))
                {
                    cmsMenu.AddItem(new ApplicationMenuItem(
                        CmsKitAdminMenus.Pages.PagesMenu,
                        l["Pages"].Value,
                        "/Cms/Pages"));
                }
            }

            if (GlobalFeatureManager.Instance.IsEnabled<BlogsFeature>())
            {
                if (await context.IsGrantedAsync(CmsKitAdminPermissions.Blogs.Default))
                {
                    cmsMenu.AddItem(new ApplicationMenuItem(
                        CmsKitAdminMenus.Blogs.BlogsMenu,
                        l["Blogs"],
                        "/Cms/Blogs"
                        ));
                }

                if (await context.IsGrantedAsync(CmsKitAdminPermissions.BlogPosts.Default))
                {
                    cmsMenu.AddItem(new ApplicationMenuItem(
                        CmsKitAdminMenus.BlogPosts.BlogPostsMenu,
                        l["BlogPosts"],
                        "/Cms/BlogPosts"
                        ));
                }
            }

            if (cmsMenu.Items.Count > 0)
            {
                context.Menu.AddItem(cmsMenu);
            }
        }
    }
}