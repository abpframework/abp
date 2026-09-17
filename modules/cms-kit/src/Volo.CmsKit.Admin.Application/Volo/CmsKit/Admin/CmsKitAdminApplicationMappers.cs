using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;
using Volo.CmsKit.Admin.Blogs;
using Volo.CmsKit.Admin.Comments;
using Volo.CmsKit.Admin.MediaDescriptors;
using Volo.CmsKit.Admin.Pages;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.Admin.Tags;
using Volo.CmsKit.Comments;
using Volo.CmsKit.MediaDescriptors;
using Volo.CmsKit.Pages;
using Volo.CmsKit.Tags;
using Volo.CmsKit.Users;
using Volo.CmsKit.Menus;
using Volo.CmsKit.Admin.Menus;

namespace Volo.CmsKit.Admin;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class TagEntityTypeDefinitonToTagDefinitionDtoMapper : MapperBase<TagEntityTypeDefiniton, TagDefinitionDto>
{
    public override partial TagDefinitionDto Map(TagEntityTypeDefiniton source);

    public override partial void Map(TagEntityTypeDefiniton source, TagDefinitionDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class BlogPostToBlogPostDtoMapper : MapperBase<BlogPost, BlogPostDto>
{
    public override partial BlogPostDto Map(BlogPost source);

    public override partial void Map(BlogPost source, BlogPostDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class BlogPostToBlogPostListDtoMapper : MapperBase<BlogPost, BlogPostListDto>
{
    [MapperIgnoreTarget(nameof(BlogPostListDto.BlogName))]
    public override partial BlogPostListDto Map(BlogPost source);

    [MapperIgnoreTarget(nameof(BlogPostListDto.BlogName))]
    public override partial void Map(BlogPost source, BlogPostListDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class MenuItemToMenuItemWithDetailsDtoMapper : MapperBase<MenuItem, MenuItemWithDetailsDto>
{
    [MapperIgnoreTarget(nameof(MenuItemWithDetailsDto.PageTitle))]
    public override partial MenuItemWithDetailsDto Map(MenuItem source);

    [MapperIgnoreTarget(nameof(MenuItemWithDetailsDto.PageTitle))]
    public override partial void Map(MenuItem source, MenuItemWithDetailsDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class MenuItemToMenuItemMapper : MapperBase<MenuItem, MenuItemDto>
{
    public override partial MenuItemDto Map(MenuItem source);

    public override partial void Map(MenuItem source, MenuItemDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class MediaDescriptorToMediaDescriptorDtoMapper : MapperBase<MediaDescriptor, MediaDescriptorDto>
{
    public override partial MediaDescriptorDto Map(MediaDescriptor source);

    public override partial void Map(MediaDescriptor source, MediaDescriptorDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class TagToTagDtoMapper : MapperBase<Tag, TagDto>
{
    public override partial TagDto Map(Tag source);

    public override partial void Map(Tag source, TagDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class BlogToBlogDtoMapper : MapperBase<Blog, BlogDto>
{
    [MapperIgnoreTarget(nameof(BlogDto.BlogPostCount))]
    public override partial BlogDto Map(Blog source);

    [MapperIgnoreTarget(nameof(BlogDto.BlogPostCount))]
    public override partial void Map(Blog source, BlogDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class PageToPageLookupDtoMapper : MapperBase<Page, PageLookupDto>
{
    public override partial PageLookupDto Map(Page source);

    public override partial void Map(Page source, PageLookupDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class PageToPageDtoMapper : MapperBase<Page, PageDto>
{
    public override partial PageDto Map(Page source);

    public override partial void Map(Page source, PageDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class CommentToCommentWithAuthorDtoMapper : MapperBase<Comment, CommentWithAuthorDto>
{
    [MapperIgnoreTarget(nameof(CommentWithAuthorDto.Author))]
    public override partial CommentWithAuthorDto Map(Comment source);

    [MapperIgnoreTarget(nameof(CommentWithAuthorDto.Author))]
    public override partial void Map(Comment source, CommentWithAuthorDto destination);
}
    
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class CommentToCommentDtoMapper : MapperBase<Comment, CommentDto>
{
    public override partial CommentDto Map(Comment source);

    public override partial void Map(Comment source, CommentDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class CmsUserToCommentsCmsUserDtoMapper : MapperBase<CmsUser, Comments.CmsUserDto>
{
    public override partial Comments.CmsUserDto Map(CmsUser source);

    public override partial void Map(CmsUser source, Comments.CmsUserDto destination);
}