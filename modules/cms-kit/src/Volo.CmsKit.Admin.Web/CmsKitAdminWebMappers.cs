using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;
using Volo.CmsKit.Admin.Blogs;
using Volo.CmsKit.Admin.Menus;
using Volo.CmsKit.Admin.Pages;
using Volo.CmsKit.Admin.Tags;
using Volo.CmsKit.Admin.Web.Pages.CmsKit.BlogPosts;
using Volo.CmsKit.Admin.Web.Pages.CmsKit.Blogs;
using Volo.CmsKit.Admin.Web.Pages.CmsKit.Tags;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.Tags;
using CreateModalModel = Volo.CmsKit.Admin.Web.Pages.CmsKit.Tags.CreateModalModel;

namespace Volo.CmsKit.Admin.Web;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class BlogFeatureInputDtoToBlogFeatureViewModelMapper : TwoWayMapperBase<BlogFeatureInputDto, FeaturesModalModel.BlogFeatureViewModel>
{
    public override partial FeaturesModalModel.BlogFeatureViewModel Map(BlogFeatureInputDto source);
    public override partial void Map(BlogFeatureInputDto source, FeaturesModalModel.BlogFeatureViewModel destination);

    public override partial BlogFeatureInputDto ReverseMap(FeaturesModalModel.BlogFeatureViewModel destination);
    public override partial void ReverseMap(FeaturesModalModel.BlogFeatureViewModel destination, BlogFeatureInputDto source);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class BlogFeatureDtoToBlogFeatureViewModelMapper : TwoWayMapperBase<BlogFeatureDto, FeaturesModalModel.BlogFeatureViewModel>
{
    public override partial FeaturesModalModel.BlogFeatureViewModel Map(BlogFeatureDto source);
    public override partial void Map(BlogFeatureDto source, FeaturesModalModel.BlogFeatureViewModel destination);

    public override partial BlogFeatureDto ReverseMap(FeaturesModalModel.BlogFeatureViewModel destination);
    public override partial void ReverseMap(FeaturesModalModel.BlogFeatureViewModel destination, BlogFeatureDto source);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class CreateBlogPostDtoToCreateBlogPostViewModelMapper : TwoWayMapperBase<CreateBlogPostDto, CreateModel.CreateBlogPostViewModel>
{
    public override partial CreateModel.CreateBlogPostViewModel Map(CreateBlogPostDto source);
    public override partial void Map(CreateBlogPostDto source, CreateModel.CreateBlogPostViewModel destination);

    public override partial CreateBlogPostDto ReverseMap(CreateModel.CreateBlogPostViewModel destination);
    public override partial void ReverseMap(CreateModel.CreateBlogPostViewModel destination, CreateBlogPostDto source);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class TagUpdateDtoToTagEditViewModelMapper : TwoWayMapperBase<TagUpdateDto, EditModalModel.TagEditViewModel>
{
    public override partial EditModalModel.TagEditViewModel Map(TagUpdateDto source);
    public override partial void Map(TagUpdateDto source, EditModalModel.TagEditViewModel destination);

    public override partial TagUpdateDto ReverseMap(EditModalModel.TagEditViewModel destination);
    public override partial void ReverseMap(EditModalModel.TagEditViewModel destination, TagUpdateDto source);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class TagDtoToCreateBlogPostDtoMapper : MapperBase<TagDto, EditModalModel.TagEditViewModel>
{
    public override partial EditModalModel.TagEditViewModel Map(TagDto source);

    public override partial void Map(TagDto source, EditModalModel.TagEditViewModel destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class CreateBlogPostViewModelToCreateBlogPostDtoMapper : MapperBase<CreateModel.CreateBlogPostViewModel, CreateBlogPostDto>
{
    public override partial CreateBlogPostDto Map(CreateModel.CreateBlogPostViewModel source);

    public override partial void Map(CreateModel.CreateBlogPostViewModel source, CreateBlogPostDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class UpdateBlogPostViewModelToUpdateBlogPostDtoMapper : TwoWayMapperBase<UpdateModel.UpdateBlogPostViewModel, UpdateBlogPostDto>
{
    public override partial UpdateBlogPostDto Map(UpdateModel.UpdateBlogPostViewModel source);

    public override partial void Map(UpdateModel.UpdateBlogPostViewModel source, UpdateBlogPostDto destination);

    public override partial UpdateModel.UpdateBlogPostViewModel ReverseMap(UpdateBlogPostDto destination);

    public override partial void ReverseMap(UpdateBlogPostDto destination, UpdateModel.UpdateBlogPostViewModel source);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class BlogPostDtoToUpdateBlogPostViewModelMapper : MapperBase<BlogPostDto, UpdateModel.UpdateBlogPostViewModel>
{
    public override partial UpdateModel.UpdateBlogPostViewModel Map(BlogPostDto source);

    public override partial void Map(BlogPostDto source, UpdateModel.UpdateBlogPostViewModel destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class BlogDtoUpdateToBlogViewModelMapper : MapperBase<BlogDto, Volo.CmsKit.Admin.Web.Pages.CmsKit.Blogs.UpdateModalModel.UpdateBlogViewModel>
{
    public override partial Volo.CmsKit.Admin.Web.Pages.CmsKit.Blogs.UpdateModalModel.UpdateBlogViewModel Map(BlogDto source);

    public override partial void Map(BlogDto source, Volo.CmsKit.Admin.Web.Pages.CmsKit.Blogs.UpdateModalModel.UpdateBlogViewModel destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class CreateBlogViewModelToCreateBlogDtoMapper : MapperBase<Volo.CmsKit.Admin.Web.Pages.CmsKit.Blogs.CreateModalModel.CreateBlogViewModel, CreateBlogDto>
{
    public override partial CreateBlogDto Map(Volo.CmsKit.Admin.Web.Pages.CmsKit.Blogs.CreateModalModel.CreateBlogViewModel source);

    public override partial void Map(Volo.CmsKit.Admin.Web.Pages.CmsKit.Blogs.CreateModalModel.CreateBlogViewModel source, CreateBlogDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class UpdateBlogViewModelToUpdateBlogDtoMapper : MapperBase<Volo.CmsKit.Admin.Web.Pages.CmsKit.Blogs.UpdateModalModel.UpdateBlogViewModel, UpdateBlogDto>
{
    public override partial UpdateBlogDto Map(Volo.CmsKit.Admin.Web.Pages.CmsKit.Blogs.UpdateModalModel.UpdateBlogViewModel source);

    public override partial void Map(Volo.CmsKit.Admin.Web.Pages.CmsKit.Blogs.UpdateModalModel.UpdateBlogViewModel source, UpdateBlogDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class MenuItemUpdateViewModelToMenuItemCreateInputMapper : TwoWayMapperBase<Volo.CmsKit.Admin.Web.Pages.CmsKit.Menus.MenuItems.CreateModalModel.MenuItemCreateViewModel, MenuItemCreateInput>
{
    public override partial MenuItemCreateInput Map(Volo.CmsKit.Admin.Web.Pages.CmsKit.Menus.MenuItems.CreateModalModel.MenuItemCreateViewModel source);

    public override partial void Map(Volo.CmsKit.Admin.Web.Pages.CmsKit.Menus.MenuItems.CreateModalModel.MenuItemCreateViewModel source, MenuItemCreateInput destination);
    public override partial Pages.CmsKit.Menus.MenuItems.CreateModalModel.MenuItemCreateViewModel ReverseMap(MenuItemCreateInput destination);

    public override partial void ReverseMap(MenuItemCreateInput destination, Pages.CmsKit.Menus.MenuItems.CreateModalModel.MenuItemCreateViewModel source);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class MenuItemUpdateViewModelToMenuItemUpdateInputMapper : MapperBase<Volo.CmsKit.Admin.Web.Pages.CmsKit.Menus.MenuItems.UpdateModalModel.MenuItemUpdateViewModel, MenuItemUpdateInput>
{
    public override partial MenuItemUpdateInput Map(Volo.CmsKit.Admin.Web.Pages.CmsKit.Menus.MenuItems.UpdateModalModel.MenuItemUpdateViewModel source);

    public override partial void Map(Volo.CmsKit.Admin.Web.Pages.CmsKit.Menus.MenuItems.UpdateModalModel.MenuItemUpdateViewModel source, MenuItemUpdateInput destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class MenuItemWithDetailsDtoToMenuItemUpdateViewModelMapper : MapperBase<MenuItemWithDetailsDto, Volo.CmsKit.Admin.Web.Pages.CmsKit.Menus.MenuItems.UpdateModalModel.MenuItemUpdateViewModel>
{
    public override partial Volo.CmsKit.Admin.Web.Pages.CmsKit.Menus.MenuItems.UpdateModalModel.MenuItemUpdateViewModel Map(MenuItemWithDetailsDto source);

    public override partial void Map(MenuItemWithDetailsDto source, Volo.CmsKit.Admin.Web.Pages.CmsKit.Menus.MenuItems.UpdateModalModel.MenuItemUpdateViewModel destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class PageDtoToCreatePageInputDtoMapper : MapperBase<PageDto, Volo.CmsKit.Admin.Web.Pages.CmsKit.Pages.UpdateModel.UpdatePageViewModel>
{
    public override partial Volo.CmsKit.Admin.Web.Pages.CmsKit.Pages.UpdateModel.UpdatePageViewModel Map(PageDto source);

    public override partial void Map(PageDto source, Volo.CmsKit.Admin.Web.Pages.CmsKit.Pages.UpdateModel.UpdatePageViewModel destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class CreatePageViewModelToCreatePageInputDtoMapper : TwoWayMapperBase<Volo.CmsKit.Admin.Web.Pages.CmsKit.Pages.CreateModel.CreatePageViewModel, CreatePageInputDto>
{
    public override partial CreatePageInputDto Map(Volo.CmsKit.Admin.Web.Pages.CmsKit.Pages.CreateModel.CreatePageViewModel source);

    public override partial void Map(Volo.CmsKit.Admin.Web.Pages.CmsKit.Pages.CreateModel.CreatePageViewModel source, CreatePageInputDto destination);
    public override partial Pages.CmsKit.Pages.CreateModel.CreatePageViewModel ReverseMap(CreatePageInputDto destination);

    public override partial void ReverseMap(CreatePageInputDto destination, Pages.CmsKit.Pages.CreateModel.CreatePageViewModel source);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class UpdatePageViewModelToUpdatePageInputDtoMapper : TwoWayMapperBase<Volo.CmsKit.Admin.Web.Pages.CmsKit.Pages.UpdateModel.UpdatePageViewModel, UpdatePageInputDto>
{
    public override partial UpdatePageInputDto Map(Volo.CmsKit.Admin.Web.Pages.CmsKit.Pages.UpdateModel.UpdatePageViewModel source);

    public override partial void Map(Volo.CmsKit.Admin.Web.Pages.CmsKit.Pages.UpdateModel.UpdatePageViewModel source, UpdatePageInputDto destination);

    public override partial Pages.CmsKit.Pages.UpdateModel.UpdatePageViewModel ReverseMap(UpdatePageInputDto destination);

    public override partial void ReverseMap(UpdatePageInputDto destination, Pages.CmsKit.Pages.UpdateModel.UpdatePageViewModel source);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class TagCreateViewModelToTagCreateDtoMapper : TwoWayMapperBase<CreateModalModel.TagCreateViewModel, TagCreateDto>
{
    public override partial TagCreateDto Map(CreateModalModel.TagCreateViewModel source);

    public override partial void Map(CreateModalModel.TagCreateViewModel source, TagCreateDto destination);
    public override partial CreateModalModel.TagCreateViewModel ReverseMap(TagCreateDto destination);

    public override partial void ReverseMap(TagCreateDto destination, CreateModalModel.TagCreateViewModel source);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class TagEditViewModelToTagUpdateDtoMapper : MapperBase<EditModalModel.TagEditViewModel, TagUpdateDto>
{
    public override partial TagUpdateDto Map(EditModalModel.TagEditViewModel source);

    public override partial void Map(EditModalModel.TagEditViewModel source, TagUpdateDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class TagDtoToTagEditViewModelMapper : MapperBase<TagDto, EditModalModel.TagEditViewModel>
{
    public override partial EditModalModel.TagEditViewModel Map(TagDto source);

    public override partial void Map(TagDto source, EditModalModel.TagEditViewModel destination);
}