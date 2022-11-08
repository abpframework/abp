﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Features;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.Blogs;

[RequiresFeature(CmsKitFeatures.BlogEnable)]
[RequiresGlobalFeature(typeof(BlogsFeature))]
[RemoteService(Name = CmsKitAdminRemoteServiceConsts.RemoteServiceName)]
[Area(CmsKitAdminRemoteServiceConsts.ModuleName)]
[Authorize(CmsKitAdminPermissions.BlogPosts.Default)]
[Route("api/cms-kit-admin/blogs/blog-posts")]
public class BlogPostAdminController : CmsKitAdminController, IBlogPostAdminAppService
{
    protected readonly IBlogPostAdminAppService BlogPostAdminAppService;

    public BlogPostAdminController(IBlogPostAdminAppService blogPostAdminAppService)
    {
        BlogPostAdminAppService = blogPostAdminAppService;
    }

    [HttpPost]
    [Authorize(CmsKitAdminPermissions.BlogPosts.Create)]
    public virtual Task<BlogPostDto> CreateAsync(CreateBlogPostDto input)
    {
        return BlogPostAdminAppService.CreateAsync(input);
    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize(CmsKitAdminPermissions.BlogPosts.Delete)]
    public virtual Task DeleteAsync(Guid id)
    {
        return BlogPostAdminAppService.DeleteAsync(id);
    }

    [HttpGet]
    [Route("{id:Guid}")]
    [Authorize(CmsKitAdminPermissions.BlogPosts.Default)]
    public virtual Task<BlogPostDto> GetAsync(Guid id)
    {
        return BlogPostAdminAppService.GetAsync(id);
    }

    [HttpGet]
    [Authorize(CmsKitAdminPermissions.BlogPosts.Default)]
    public virtual Task<PagedResultDto<BlogPostListDto>> GetListAsync(BlogPostGetListInput input)
    {
        return BlogPostAdminAppService.GetListAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    [Authorize(CmsKitAdminPermissions.BlogPosts.Update)]
    public virtual Task<BlogPostDto> UpdateAsync(Guid id, UpdateBlogPostDto input)
    {
        return BlogPostAdminAppService.UpdateAsync(id, input);
    }

    [HttpPost]
    [Route("{id}/publish")]
    [Authorize(CmsKitAdminPermissions.BlogPosts.Publish)]
    public virtual Task PublishAsync(Guid id)
    {
        return BlogPostAdminAppService.PublishAsync(id);
    }

    [HttpPost]
    [Route("{id}/draft")]
    [Authorize(CmsKitAdminPermissions.BlogPosts.Update)]
    public virtual Task DraftAsync(Guid id)
    {
        return BlogPostAdminAppService.DraftAsync(id);
    }

    [HttpPost]
    [Route("create-and-publish")]
    [Authorize(CmsKitAdminPermissions.BlogPosts.Create)]
    [Authorize(CmsKitAdminPermissions.BlogPosts.Publish)]
    public virtual Task<BlogPostDto> CreateAndPublishAsync(CreateBlogPostDto input)
    {
        return BlogPostAdminAppService.CreateAndPublishAsync(input);
    }

    [HttpPost]
    [Route("{id}/send-to-review")]
    [Authorize(CmsKitAdminPermissions.BlogPosts.Create)]
    public virtual Task SendToReviewAsync(Guid id)
    {
        return BlogPostAdminAppService.SendToReviewAsync(id);
    }

    [HttpPost]
    [Route("create-and-send-to-review")]
    [Authorize(CmsKitAdminPermissions.BlogPosts.Create)]
    public virtual Task<BlogPostDto> CreateAndSendToReviewAsync(CreateBlogPostDto input)
    {
        return BlogPostAdminAppService.CreateAndSendToReviewAsync(input);
    }

    [HttpGet]
    [Route("has-blogpost-waiting-for-review")]
    [Authorize(CmsKitAdminPermissions.BlogPosts.Publish)]
    public virtual Task<bool> HasBlogPostWaitingForReviewAsync()
    {
        return BlogPostAdminAppService.HasBlogPostWaitingForReviewAsync();
    }
}