using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.Users;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Permissions;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.Admin.Blogs;

[RequiresGlobalFeature(typeof(BlogsFeature))]
[Authorize(CmsKitAdminPermissions.BlogPosts.Default)]
public class BlogPostAdminAppService : CmsKitAppServiceBase, IBlogPostAdminAppService
{
    protected BlogPostManager BlogPostManager { get; }
    protected IBlogPostRepository BlogPostRepository { get; }
    protected IBlogRepository BlogRepository { get; }
    protected ICmsUserLookupService UserLookupService { get; }

    public BlogPostAdminAppService(
        BlogPostManager blogPostManager,
        IBlogPostRepository blogPostRepository,
        IBlogRepository blogRepository,
        ICmsUserLookupService userLookupService)
    {
        BlogPostManager = blogPostManager;
        BlogPostRepository = blogPostRepository;
        BlogRepository = blogRepository;
        UserLookupService = userLookupService;
    }

    [Authorize(CmsKitAdminPermissions.BlogPosts.Create)]
    public virtual async Task<BlogPostDto> CreateAsync(CreateBlogPostDto input)
    {
        var author = await UserLookupService.GetByIdAsync(CurrentUser.GetId());

        var blog = await BlogRepository.GetAsync(input.BlogId);

        var blogPost = await BlogPostManager.CreateAsync(
            author,
            blog,
            input.Title,
            input.Slug,
            BlogPostStatus.Draft,
            input.ShortDescription,
            input.Content,
            input.CoverImageMediaId
        );

        await BlogPostRepository.InsertAsync(blogPost);

        return ObjectMapper.Map<BlogPost, BlogPostDto>(blogPost);
    }

    [Authorize(CmsKitAdminPermissions.BlogPosts.Update)]
    public virtual async Task<BlogPostDto> UpdateAsync(Guid id, UpdateBlogPostDto input)
    {
        var blogPost = await BlogPostRepository.GetAsync(id);

        blogPost.SetTitle(input.Title);
        blogPost.SetShortDescription(input.ShortDescription);
        blogPost.SetContent(input.Content);
        blogPost.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);
        blogPost.CoverImageMediaId = input.CoverImageMediaId;

        if (blogPost.Slug != input.Slug)
        {
            await BlogPostManager.SetSlugUrlAsync(blogPost, input.Slug);
        }

        await BlogPostRepository.UpdateAsync(blogPost);

        return ObjectMapper.Map<BlogPost, BlogPostDto>(blogPost);
    }

    [Authorize(CmsKitAdminPermissions.BlogPosts.Default)]
    public virtual async Task<BlogPostDto> GetAsync(Guid id)
    {
        var blogPost = await BlogPostRepository.GetAsync(id);

        return ObjectMapper.Map<BlogPost, BlogPostDto>(blogPost);
    }

    [Authorize(CmsKitAdminPermissions.BlogPosts.Default)]
    public virtual async Task<PagedResultDto<BlogPostListDto>> GetListAsync(BlogPostGetListInput input)
    {
        var blogs = (await BlogRepository.GetListAsync()).ToDictionary(x => x.Id);

        var blogPosts = await BlogPostRepository.GetListAsync(input.Filter, input.BlogId, input.AuthorId,
            statusFilter: input.Status,
            input.MaxResultCount, input.SkipCount, input.Sorting);

        var count = await BlogPostRepository.GetCountAsync(input.Filter, input.BlogId, input.AuthorId);

        var dtoList = blogPosts.Select(x =>
        {
            var dto = ObjectMapper.Map<BlogPost, BlogPostListDto>(x);
            dto.BlogName = blogs[x.BlogId].Name;

            return dto;
        }).ToList();

        return new PagedResultDto<BlogPostListDto>(count, dtoList);
    }

    [Authorize(CmsKitAdminPermissions.BlogPosts.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await BlogPostRepository.DeleteAsync(id);
    }

    [Authorize(CmsKitAdminPermissions.BlogPosts.Publish)]
    public virtual async Task PublishAsync(Guid id)
    {
        var blogPost = await BlogPostRepository.GetAsync(id);
        blogPost.SetPublished();
    }

    [Authorize(CmsKitAdminPermissions.BlogPosts.Update)]
    public virtual async Task DraftAsync(Guid id)
    {
        var blogPost = await BlogPostRepository.GetAsync(id);
        blogPost.SetDraft();
    }

    [Authorize(CmsKitAdminPermissions.BlogPosts.Create)]
    [Authorize(CmsKitAdminPermissions.BlogPosts.Publish)]
    public virtual async Task<BlogPostDto> CreateAndPublishAsync(CreateBlogPostDto input)
    {
        var blogPost = await CreateAsync(input);
        await CurrentUnitOfWork.SaveChangesAsync();
        
        await PublishAsync(blogPost.Id);
        blogPost.Status = BlogPostStatus.Published;
        return blogPost;
    }
}