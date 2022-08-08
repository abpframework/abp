using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.Contents;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.Public.Blogs;

[RequiresGlobalFeature(typeof(BlogsFeature))]
public class BlogPostPublicAppService : CmsKitPublicAppServiceBase, IBlogPostPublicAppService
{
    protected IBlogRepository BlogRepository { get; }

    protected IBlogPostRepository BlogPostRepository { get; }
    protected ContentParser ContentParser { get; }

    public BlogPostPublicAppService(
        IBlogRepository blogRepository,
        IBlogPostRepository blogPostRepository,
        ContentParser contentParser)
    {
        BlogRepository = blogRepository;
        BlogPostRepository = blogPostRepository;
        ContentParser = contentParser;
    }

    public virtual async Task<BlogPostCommonDto> GetAsync(
        [NotNull] string blogSlug, [NotNull] string blogPostSlug)
    {
        var blog = await BlogRepository.GetBySlugAsync(blogSlug);

        var blogPost = await BlogPostRepository.GetBySlugAsync(blog.Id, blogPostSlug);

        var blogPostDto = ObjectMapper.Map<BlogPost, BlogPostCommonDto>(blogPost);
        blogPostDto.ContentFragments = await ContentParser.ParseAsync(blogPost.Content);
        
        return blogPostDto;
    }

    public virtual async Task<PagedResultDto<BlogPostCommonDto>> GetListAsync([NotNull] string blogSlug, BlogPostGetListInput input)
    {
        var blog = await BlogRepository.GetBySlugAsync(blogSlug);

        var blogPosts = await BlogPostRepository.GetListAsync(null, blog.Id, input.AuthorId, input.TagId,
            BlogPostStatus.Published, input.MaxResultCount,
            input.SkipCount, input.Sorting);

        return new PagedResultDto<BlogPostCommonDto>(
            await BlogPostRepository.GetCountAsync(blogId: blog.Id, tagId: input.TagId,
                statusFilter: BlogPostStatus.Published, authorId: input.AuthorId),
            ObjectMapper.Map<List<BlogPost>, List<BlogPostCommonDto>>(blogPosts));
    }

    public virtual async Task<PagedResultDto<CmsUserDto>> GetAuthorsHasBlogPostsAsync(BlogPostFilteredPagedAndSortedResultRequestDto input)
    {
        var authors = await BlogPostRepository.GetAuthorsHasBlogPostsAsync(input.SkipCount, input.MaxResultCount, input.Sorting, input.Filter);
        var authorDtos = ObjectMapper.Map<List<CmsUser>, List<CmsUserDto>>(authors);

        return new PagedResultDto<CmsUserDto>(
            await BlogPostRepository.GetAuthorsHasBlogPostsCountAsync(input.Filter),
            authorDtos);
    }

    public async Task<CmsUserDto> GetAuthorHasBlogPostAsync(Guid id)
    {
        var author = await BlogPostRepository.GetAuthorHasBlogPostAsync(id);

        return ObjectMapper.Map<CmsUser, CmsUserDto>(author);
    }
}
