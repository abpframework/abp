using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Volo.CmsKit.Admin.Web.Menus;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;
using Volo.CmsKit.Localization;
using Volo.CmsKit.Web;
using Volo.CmsKit.Permissions;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.PageToolbars;
using Volo.Abp.Localization;
using Volo.Abp.AutoMapper;

namespace Volo.CmsKit.Admin.Web
{
    [DependsOn(
        typeof(CmsKitAdminHttpApiModule),
        typeof(CmsKitCommonWebModule)
        )]
    public class CmsKitAdminWebModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(
                    typeof(CmsKitResource),
                    typeof(CmsKitAdminWebModule).Assembly,
                    typeof(CmsKitAdminApplicationContractsModule).Assembly,
                    typeof(CmsKitCommonApplicationContractsModule).Assembly
                );
            });

            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(CmsKitAdminWebModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpNavigationOptions>(options =>
            {
                options.MenuContributors.Add(new CmsKitAdminMenuContributor());
            });

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<CmsKitAdminWebModule>("Volo.CmsKit.Admin.Web");
            });

            context.Services.AddAutoMapperObjectMapper<CmsKitAdminWebModule>();
            Configure<AbpAutoMapperOptions>(options => { options.AddMaps<CmsKitAdminWebModule>(validate: true); });

            Configure<RazorPagesOptions>(options =>
            {
                options.Conventions.AuthorizeFolder("/CmsKit/Tags/", CmsKitAdminPermissions.Tags.Default);
                options.Conventions.AuthorizeFolder("/CmsKit/Tags/CreateModal", CmsKitAdminPermissions.Tags.Create);
                options.Conventions.AuthorizeFolder("/CmsKit/Tags/UpdateModal", CmsKitAdminPermissions.Tags.Update);
                options.Conventions.AuthorizeFolder("/CmsKit/Pages", CmsKitAdminPermissions.Pages.Default);
                options.Conventions.AuthorizeFolder("/CmsKit/Pages/Create", CmsKitAdminPermissions.Pages.Create);
                options.Conventions.AuthorizeFolder("/CmsKit/Pages/Update", CmsKitAdminPermissions.Pages.Update);
                options.Conventions.AuthorizeFolder("/CmsKit/Blogs", CmsKitAdminPermissions.Blogs.Default);
                options.Conventions.AuthorizeFolder("/CmsKit/Blogs/Create", CmsKitAdminPermissions.Blogs.Create);
                options.Conventions.AuthorizeFolder("/CmsKit/Blogs/Update", CmsKitAdminPermissions.Blogs.Update);
                options.Conventions.AuthorizeFolder("/CmsKit/BlogPosts", CmsKitAdminPermissions.BlogPosts.Default);
                options.Conventions.AuthorizeFolder("/CmsKit/BlogPosts/Create", CmsKitAdminPermissions.BlogPosts.Create);
                options.Conventions.AuthorizeFolder("/CmsKit/BlogPosts/Update", CmsKitAdminPermissions.BlogPosts.Update);
                options.Conventions.AuthorizeFolder("/CmsKit/Comments/", CmsKitAdminPermissions.Comments.Default);
                options.Conventions.AuthorizeFolder("/CmsKit/Comments/Details", CmsKitAdminPermissions.Comments.Default);
                options.Conventions.AuthorizeFolder("/CmsKit/Menus", CmsKitAdminPermissions.Menus.Default);
                options.Conventions.AuthorizePage("/CmsKit/Menus/CreateModal", CmsKitAdminPermissions.Menus.Create);
                options.Conventions.AuthorizePage("/CmsKit/Menus/UpdateModal", CmsKitAdminPermissions.Menus.Update);
                options.Conventions.AuthorizeFolder("/CmsKit/Menus/MenuItems", CmsKitAdminPermissions.Menus.MenuItems.Default);
            });

            Configure<RazorPagesOptions>(options =>
            {
                options.Conventions.AddPageRoute("/CmsKit/Tags/Index", "/Cms/Tags");
                options.Conventions.AddPageRoute("/CmsKit/Pages/Index", "/Cms/Pages");
                options.Conventions.AddPageRoute("/CmsKit/Pages/Create", "/Cms/Pages/Create");
                options.Conventions.AddPageRoute("/CmsKit/Pages/Update", "/Cms/Pages/Update/{Id}");
                options.Conventions.AddPageRoute("/CmsKit/Blogs/Index", "/Cms/Blogs");
                options.Conventions.AddPageRoute("/CmsKit/BlogPosts/Index", "/Cms/BlogPosts");
                options.Conventions.AddPageRoute("/CmsKit/BlogPosts/Create", "/Cms/BlogPosts/Create");
                options.Conventions.AddPageRoute("/CmsKit/BlogPosts/Update", "/Cms/BlogPosts/Update/{Id}");
                options.Conventions.AddPageRoute("/CmsKit/Comments/Index", "/Cms/Comments");
                options.Conventions.AddPageRoute("/CmsKit/Comments/Details", "/Cms/Comments/{Id}");
                options.Conventions.AddPageRoute("/CmsKit/Menus/Index", "/Cms/Menus");
                options.Conventions.AddPageRoute("/CmsKit/Menus/MenuItems/Index", "/Cms/Menus/{Id}/menu-items");
            });

            Configure<AbpPageToolbarOptions>(options =>
            {

                options.Configure<Volo.CmsKit.Admin.Web.Pages.CmsKit.Tags.IndexModel>(
                    toolbar =>
                    {
                        toolbar.AddButton(
                            LocalizableString.Create<CmsKitResource>("NewTag"),
                            icon: "plus",
                            name: "NewButton",
                            requiredPolicyName: CmsKitAdminPermissions.Tags.Create
                        );
                    }
                );

                options.Configure<Volo.CmsKit.Admin.Web.Pages.CmsKit.Pages.IndexModel>(
                    toolbar =>
                    {
                        toolbar.AddButton(
                            LocalizableString.Create<CmsKitResource>("NewPage"),
                            icon: "plus",
                            name: "CreatePage",
                            requiredPolicyName: CmsKitAdminPermissions.Pages.Create
                        );
                    });

                options.Configure<Volo.CmsKit.Admin.Web.Pages.CmsKit.Blogs.IndexModel>(
                    toolbar =>
                    {
                        toolbar.AddButton(
                            LocalizableString.Create<CmsKitResource>("NewBlog"),
                            icon: "plus",
                            name: "CreateBlog",
                            id: "CreateBlog",
                            requiredPolicyName: CmsKitAdminPermissions.Blogs.Create
                            );
                    });

                options.Configure<Volo.CmsKit.Admin.Web.Pages.CmsKit.BlogPosts.IndexModel>(
                    toolbar =>
                    {
                        toolbar.AddButton(
                            LocalizableString.Create<CmsKitResource>("NewBlogPost"),
                            icon: "plus",
                            name: "CreateBlogPost",
                            id: "CreateBlogPost",
                            requiredPolicyName: CmsKitAdminPermissions.BlogPosts.Create
                            );
                    });

                options.Configure<Volo.CmsKit.Admin.Web.Pages.CmsKit.Menus.IndexModel>(
                    toolbar =>
                    {
                        toolbar.AddButton(
                            LocalizableString.Create<CmsKitResource>("NewMenu"),
                            icon: "plus",
                            name: "CreateMenu",
                            id: "CreateMenu",
                            requiredPolicyName: CmsKitAdminPermissions.Menus.Create
                            );
                    });

                options.Configure<Volo.CmsKit.Admin.Web.Pages.CmsKit.Menus.MenuItems.IndexModel>(
                    toolbar =>
                    {
                        toolbar.AddButton(
                            LocalizableString.Create<CmsKitResource>("NewMenuItem"),
                            icon: "plus",
                            name: "CreateMenuItem",
                            id: "CreateMenuItem",
                            requiredPolicyName: CmsKitAdminPermissions.Menus.MenuItems.Create
                            );
                    });
            });
        }
    }
}
