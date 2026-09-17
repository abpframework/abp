using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;
using Volo.Blogging.Blogs;
using Volo.Blogging.Comments;
using Volo.Blogging.Posts;
using Volo.Blogging.Tagging;

namespace Volo.Blogging;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class TagToTagEtoMapper : MapperBase<Tag, TagEto>
{
    public override partial TagEto Map(Tag source);

    public override partial void Map(Tag source, TagEto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class PostToPostEtoMapper : MapperBase<Post, PostEto>
{
    public override partial PostEto Map(Post source);

    public override partial void Map(Post source, PostEto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class CommentToCommentEtoMapper : MapperBase<Comment, CommentEto>
{
    public override partial CommentEto Map(Comment source);

    public override partial void Map(Comment source, CommentEto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class BlogToBlogEtoMapper : MapperBase<Blog, BlogEto>
{
    public override partial BlogEto Map(Blog source);

    public override partial void Map(Blog source, BlogEto destination);
}