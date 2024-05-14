using AutoMapper;
using Volo.Abp.AutoMapper;
using Volo.CmsKit.Admin.Blogs;
using Volo.CmsKit.Admin.Menus;
using Volo.CmsKit.Admin.Pages;
using Volo.CmsKit.Admin.Tags;
using Volo.CmsKit.Menus;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit.Admin.Web;

public class CmsKitAdminWebAutoMapperProfile : Profile
{
    public CmsKitAdminWebAutoMapperProfile()
    {
        CreateBlogPostMappings();
        CreateBlogMappings();
        CreateMenuMappings();
        CreatePageMappings();
        CreateTagMappings();
    }

    protected virtual void CreateBlogPostMappings()
    {
        CreateMap<Volo.CmsKit.Admin.Web.Pages.CmsKit.BlogPosts.CreateModel.CreateBlogPostViewModel, CreateBlogPostDto>().MapExtraProperties();
        CreateMap<Volo.CmsKit.Admin.Web.Pages.CmsKit.BlogPosts.UpdateModel.UpdateBlogPostViewModel, UpdateBlogPostDto>().MapExtraProperties();
        CreateMap<BlogPostDto, Volo.CmsKit.Admin.Web.Pages.CmsKit.BlogPosts.UpdateModel.UpdateBlogPostViewModel>().MapExtraProperties();
    }
    
    protected virtual void CreateBlogMappings()
    {
        CreateMap<Volo.CmsKit.Admin.Web.Pages.CmsKit.Blogs.CreateModalModel.CreateBlogViewModel, CreateBlogDto>().MapExtraProperties();
        CreateMap<Volo.CmsKit.Admin.Web.Pages.CmsKit.Blogs.UpdateModalModel.UpdateBlogViewModel, UpdateBlogDto>().MapExtraProperties();
        CreateMap<BlogDto, Volo.CmsKit.Admin.Web.Pages.CmsKit.Blogs.UpdateModalModel.UpdateBlogViewModel>().MapExtraProperties();
    }

    protected virtual void CreateMenuMappings()
    {
        CreateMap<Volo.CmsKit.Admin.Web.Pages.CmsKit.Menus.MenuItems.CreateModalModel.MenuItemCreateViewModel, MenuItemCreateInput>().MapExtraProperties();
        CreateMap<Volo.CmsKit.Admin.Web.Pages.CmsKit.Menus.MenuItems.UpdateModalModel.MenuItemUpdateViewModel, MenuItemUpdateInput>().MapExtraProperties();
        CreateMap<MenuItemWithDetailsDto, Volo.CmsKit.Admin.Web.Pages.CmsKit.Menus.MenuItems.UpdateModalModel.MenuItemUpdateViewModel>().MapExtraProperties();
    }

    protected virtual void CreatePageMappings()
    {
        CreateMap<Volo.CmsKit.Admin.Web.Pages.CmsKit.Pages.CreateModel.CreatePageViewModel, CreatePageInputDto>().MapExtraProperties();
        CreateMap<Volo.CmsKit.Admin.Web.Pages.CmsKit.Pages.UpdateModel.UpdatePageViewModel, UpdatePageInputDto>().MapExtraProperties();
        CreateMap<PageDto, Volo.CmsKit.Admin.Web.Pages.CmsKit.Pages.UpdateModel.UpdatePageViewModel>().MapExtraProperties();
    }

    protected virtual void CreateTagMappings()
    {
        CreateMap<Volo.CmsKit.Admin.Web.Pages.CmsKit.Tags.CreateModalModel.TagCreateViewModel, TagCreateDto>().MapExtraProperties();
        CreateMap<Volo.CmsKit.Admin.Web.Pages.CmsKit.Tags.EditModalModel.TagEditViewModel, TagUpdateDto>().MapExtraProperties();
        CreateMap<TagDto, Volo.CmsKit.Admin.Web.Pages.CmsKit.Tags.EditModalModel.TagEditViewModel>().MapExtraProperties();
    }
}