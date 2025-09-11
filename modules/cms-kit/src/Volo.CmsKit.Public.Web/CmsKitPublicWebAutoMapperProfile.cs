using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;
using Volo.CmsKit.Contents;
using Volo.CmsKit.Public.Comments;
using Volo.CmsKit.Public.Web.Pages.Public.CmsKit.Blogs;
using Volo.CmsKit.Public.Web.Pages.Public.CmsKit.Pages;

namespace Volo.CmsKit.Public.Web;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class PageDtoToPageViewModelMapper : TwoWayMapperBase<PageDto, PageViewModel>
{
    public override partial PageViewModel Map(PageDto source);
    public override partial void Map(PageDto source, PageViewModel destination);

    public override partial PageDto ReverseMap(PageViewModel destination);
    public override partial void ReverseMap(PageViewModel destination, PageDto source);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class BlogPostCommonDtoToBlogPostViewModelMapper : TwoWayMapperBase<BlogPostCommonDto, BlogPostViewModel>
{
    public override partial BlogPostViewModel Map(BlogPostCommonDto source);
    public override partial void Map(BlogPostCommonDto source, BlogPostViewModel destination);

    public override partial BlogPostCommonDto ReverseMap(BlogPostViewModel destination);
    public override partial void ReverseMap(BlogPostViewModel destination, BlogPostCommonDto source);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class CreateCommentWithParametersInputToCommentDtoMapper : MapperBase<CreateCommentWithParametersInput, CreateCommentInput>
{
    [MapperIgnoreTarget(nameof(CreateCommentInput.ExtraProperties))]
    public override partial CreateCommentInput Map(CreateCommentWithParametersInput source);

    [MapperIgnoreTarget(nameof(CreateCommentInput.ExtraProperties))]
    public override partial void Map(CreateCommentWithParametersInput source, CreateCommentInput destination);
}