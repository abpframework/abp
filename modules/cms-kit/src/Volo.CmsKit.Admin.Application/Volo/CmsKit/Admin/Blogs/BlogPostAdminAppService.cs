﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.Users;
using Volo.CmsKit.Admin.MediaDescriptors;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.Features;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Permissions;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.Admin.Blogs;

[RequiresFeature(CmsKitFeatures.BlogEnable)]
[RequiresGlobalFeature(typeof(BlogsFeature))]
[Authorize(CmsKitAdminPermissions.BlogPosts.Default)]
public class BlogPostAdminAppService : CmsKitAppServiceBase, IBlogPostAdminAppService
{
    protected BlogPostManager BlogPostManager { get; }
    protected IBlogPostRepository BlogPostRepository { get; }
    protected IBlogRepository BlogRepository { get; }
    protected ICmsUserLookupService UserLookupService { get; }

    protected IMediaDescriptorAdminAppService MediaDescriptorAdminAppService { get; }

    public BlogPostAdminAppService(
        BlogPostManager blogPostManager,
        IBlogPostRepository blogPostRepository,
        IBlogRepository blogRepository,
        ICmsUserLookupService userLookupService,
        IMediaDescriptorAdminAppService mediaDescriptorAdminAppService)
    {
        BlogPostManager = blogPostManager;
        BlogPostRepository = blogPostRepository;
        BlogRepository = blogRepository;
        UserLookupService = userLookupService;
        MediaDescriptorAdminAppService = mediaDescriptorAdminAppService;
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

        if (blogPost.CoverImageMediaId != null && input.CoverImageMediaId == null)
        {
            await MediaDescriptorAdminAppService.DeleteAsync(blogPost.CoverImageMediaId.Value);
        }
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

        var blogPosts = await BlogPostRepository.GetListAsync(input.Filter, input.BlogId, input.AuthorId, input.TagId,
            statusFilter: input.Status,
            input.MaxResultCount, input.SkipCount, input.Sorting);

        var count = await BlogPostRepository.GetCountAsync(input.Filter, input.BlogId, input.AuthorId, tagId: input.TagId);

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
        await BlogPostRepository.UpdateAsync(blogPost);
    }

    [Authorize(CmsKitAdminPermissions.BlogPosts.Update)]
    public virtual async Task DraftAsync(Guid id)
    {
        var blogPost = await BlogPostRepository.GetAsync(id);
        blogPost.SetDraft();
        await BlogPostRepository.UpdateAsync(blogPost);
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

    [Authorize(CmsKitAdminPermissions.BlogPosts.Create)]
    public virtual async Task SendToReviewAsync(Guid id)
    {
        var blogPost = await BlogPostRepository.GetAsync(id);
        blogPost.SetWaitingForReview();
        await BlogPostRepository.UpdateAsync(blogPost);
    }

    [Authorize(CmsKitAdminPermissions.BlogPosts.Create)]
    public virtual async Task<BlogPostDto> CreateAndSendToReviewAsync(CreateBlogPostDto input)
    {
        var blogPost = await CreateAsync(input);
        await CurrentUnitOfWork.SaveChangesAsync();

        await SendToReviewAsync(blogPost.Id);
        blogPost.Status = BlogPostStatus.WaitingForReview;
        return blogPost;
    }

    [Authorize(CmsKitAdminPermissions.BlogPosts.Publish)]
    public async Task<bool> HasBlogPostWaitingForReviewAsync()
    {
        return await BlogPostRepository.HasBlogPostWaitingForReviewAsync();
    }
}