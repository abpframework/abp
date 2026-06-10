using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.Comments;
using Volo.CmsKit.Contents;
using Volo.CmsKit.GlobalResources;
using Volo.CmsKit.Menus;
using Volo.CmsKit.Pages;
using Volo.CmsKit.Public.Comments;
using Volo.CmsKit.Public.GlobalResources;
using Volo.CmsKit.Public.Ratings;
using Volo.CmsKit.Ratings;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.Public;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class CmsUserToCmsUserDtoMapper : MapperBase<CmsUser, Comments.CmsUserDto>
{
    public override partial Comments.CmsUserDto Map(CmsUser source);

    public override partial void Map(CmsUser source, Comments.CmsUserDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class CommentToCommentDtoMapper : MapperBase<Comment, CommentDto>
{
    [MapperIgnoreTarget(nameof(CommentDto.Author))]
    public override partial CommentDto Map(Comment source);

    [MapperIgnoreTarget(nameof(CommentDto.Author))]
    public override partial void Map(Comment source, CommentDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class CommentToCommentWithDetailsDtoMapper : MapperBase<Comment, CommentWithDetailsDto>
{
    [MapperIgnoreTarget(nameof(CommentWithDetailsDto.Replies))]
    [MapperIgnoreTarget(nameof(CommentWithDetailsDto.Author))]
    public override partial CommentWithDetailsDto Map(Comment source);

    [MapperIgnoreTarget(nameof(CommentWithDetailsDto.Replies))]
    [MapperIgnoreTarget(nameof(CommentWithDetailsDto.Author))]
    public override partial void Map(Comment source, CommentWithDetailsDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class RatingToRatingDtoMapper : MapperBase<Rating, RatingDto>
{
    public override partial RatingDto Map(Rating source);

    public override partial void Map(Rating source, RatingDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class PageToPageCacheItemMapper : MapperBase<Page, PageCacheItem>
{
    public override partial PageCacheItem Map(Page source);

    public override partial void Map(Page source, PageCacheItem destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class PageCacheItemToPageDtoMapper : MapperBase<PageCacheItem, PageDto>
{
    public override partial PageDto Map(PageCacheItem source);

    public override partial void Map(PageCacheItem source, PageDto destination);
}
        
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class BlogPostToPageDtoMapper : MapperBase<Page, PageDto>
{
    public override partial PageDto Map(Page source);

    public override partial void Map(Page source, PageDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class BlogPostToBlogPostCommonDtoMapper : MapperBase<BlogPost, BlogPostCommonDto>
{
    public override partial BlogPostCommonDto Map(BlogPost source);

    public override partial void Map(BlogPost source, BlogPostCommonDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class MenuItemToMenuItemDtoMapper : MapperBase<MenuItem, MenuItemDto>
{
    public override partial MenuItemDto Map(MenuItem source);

    public override partial void Map(MenuItem source, MenuItemDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class GlobalResourceToGlobalResourceDtoMapper : MapperBase<GlobalResource, GlobalResourceDto>
{
    public override partial GlobalResourceDto Map(GlobalResource source);

    public override partial void Map(GlobalResource source, GlobalResourceDto destination);
}