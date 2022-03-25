using JetBrains.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.Public.Blogs;

[RequiresGlobalFeature(typeof(BlogsFeature))]
public class BlogPostPublicAppService : CmsKitPublicAppServiceBase, IBlogPostPublicAppService
{
    protected IBlogRepository BlogRepository { get; }

    protected IBlogPostRepository BlogPostRepository { get; }
    

    public BlogPostPublicAppService(
        IBlogRepository blogRepository,
        IBlogPostRepository blogPostRepository)
    {
        BlogRepository = blogRepository;
        BlogPostRepository = blogPostRepository;
    }

    public virtual async Task<BlogPostPublicDto> GetAsync([NotNull] string blogSlug, [NotNull] string blogPostSlug)
    {
        var blog = await BlogRepository.GetBySlugAsync(blogSlug);

        var blogPost = await BlogPostRepository.GetBySlugAsync(blog.Id, blogPostSlug);

        return ObjectMapper.Map<BlogPost, BlogPostPublicDto>(blogPost);
    }

    public virtual async Task<PagedResultDto<BlogPostPublicDto>> GetListAsync([NotNull] string blogSlug, BlogPostGetListInput input)
    {
        var blog = await BlogRepository.GetBySlugAsync(blogSlug);

        var blogPosts = await BlogPostRepository.GetListAsync(null, blog.Id, input.AuthorId, BlogPostStatus.Published, input.MaxResultCount,
            input.SkipCount, input.Sorting);

        return new PagedResultDto<BlogPostPublicDto>(
            await BlogPostRepository.GetCountAsync(blogId: blog.Id, statusFilter: BlogPostStatus.Published, authorId: input.AuthorId),
            ObjectMapper.Map<List<BlogPost>, List<BlogPostPublicDto>>(blogPosts));
    }

    public virtual async Task<List<CmsUserDto>> GetAuthorsHasBlogPostsAsync()
    {
        var authors = await BlogPostRepository.GetAuthorsHasBlogPosts();
        return ObjectMapper.Map<List<CmsUser>, List<CmsUserDto>>(authors);
    }
}
