using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;
using Volo.Blogging.Blogs;
using Volo.Blogging.Blogs.Dtos;
using Volo.Blogging.Comments;
using Volo.Blogging.Comments.Dtos;
using Volo.Blogging.Posts;
using Volo.Blogging.Tagging;
using Volo.Blogging.Tagging.Dtos;
using Volo.Blogging.Users;

namespace Volo.Blogging;
    
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class PostCacheItemToPostWithDetailsDtoMapper : MapperBase<PostCacheItem, PostWithDetailsDto>
{
    [MapperIgnoreTarget(nameof(PostWithDetailsDto.LastModificationTime))]
    [MapperIgnoreTarget(nameof(PostWithDetailsDto.LastModifierId))]
    [MapperIgnoreTarget(nameof(PostWithDetailsDto.IsDeleted))]
    [MapperIgnoreTarget(nameof(PostWithDetailsDto.DeletionTime))]
    [MapperIgnoreTarget(nameof(PostWithDetailsDto.DeleterId))]
    [MapperIgnoreTarget(nameof(PostWithDetailsDto.ConcurrencyStamp))]
    [MapperIgnoreTarget(nameof(PostWithDetailsDto.Writer))]
    [MapperIgnoreTarget(nameof(PostWithDetailsDto.CommentCount))]
    [MapperIgnoreTarget(nameof(PostWithDetailsDto.Tags))]
    public override partial PostWithDetailsDto Map(PostCacheItem source);

    [MapperIgnoreTarget(nameof(PostWithDetailsDto.LastModificationTime))]
    [MapperIgnoreTarget(nameof(PostWithDetailsDto.LastModifierId))]
    [MapperIgnoreTarget(nameof(PostWithDetailsDto.IsDeleted))]
    [MapperIgnoreTarget(nameof(PostWithDetailsDto.DeletionTime))]
    [MapperIgnoreTarget(nameof(PostWithDetailsDto.DeleterId))]
    [MapperIgnoreTarget(nameof(PostWithDetailsDto.ConcurrencyStamp))]
    [MapperIgnoreTarget(nameof(PostWithDetailsDto.Writer))]
    [MapperIgnoreTarget(nameof(PostWithDetailsDto.CommentCount))]
    [MapperIgnoreTarget(nameof(PostWithDetailsDto.Tags))]
    public override partial void Map(PostCacheItem source, PostWithDetailsDto destination);
}
    
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class PostToPostCacheItemMapper : MapperBase<Post, PostCacheItem>
{
    [MapperIgnoreTarget(nameof(PostCacheItem.CommentCount))]
    [MapperIgnoreTarget(nameof(PostCacheItem.Tags))]
    public override partial PostCacheItem Map(Post source);

    [MapperIgnoreTarget(nameof(PostCacheItem.CommentCount))]
    [MapperIgnoreTarget(nameof(PostCacheItem.Tags))]
    public override partial void Map(Post source, PostCacheItem destination);
}
    
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class CommentToCommentWithDetailsDtoMapper : MapperBase<Comment, CommentWithDetailsDto>
{
    [MapperIgnoreTarget(nameof(CommentWithDetailsDto.Writer))]
    public override partial CommentWithDetailsDto Map(Comment source);

    [MapperIgnoreTarget(nameof(CommentWithDetailsDto.Writer))]
    public override partial void Map(Comment source, CommentWithDetailsDto destination);
}
    
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class PostToPostWithDetailsDtoMapper : MapperBase<Post, PostWithDetailsDto>
{
    [MapperIgnoreTarget(nameof(PostWithDetailsDto.Tags))]
    [MapperIgnoreTarget(nameof(PostWithDetailsDto.CommentCount))]
    [MapperIgnoreTarget(nameof(PostWithDetailsDto.Writer))]
    public override partial PostWithDetailsDto Map(Post source);

    [MapperIgnoreTarget(nameof(PostWithDetailsDto.Tags))]
    [MapperIgnoreTarget(nameof(PostWithDetailsDto.CommentCount))]
    [MapperIgnoreTarget(nameof(PostWithDetailsDto.Writer))]
    public override partial void Map(Post source, PostWithDetailsDto destination);
}
    
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class TagToTagDtoMapper : MapperBase<Tag, TagDto>
{
    public override partial TagDto Map(Tag source);

    public override partial void Map(Tag source, TagDto destination);
}
    
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class BlogUserToBlogUserDtoMapper : MapperBase<BlogUser, BlogUserDto>
{
    public override partial BlogUserDto Map(BlogUser source);

    public override partial void Map(BlogUser source, BlogUserDto destination);
}
    
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class BlogToBlogDtoMapper : MapperBase<Blog, BlogDto>
{
    public override partial BlogDto Map(Blog source);

    public override partial void Map(Blog source, BlogDto destination);
}
