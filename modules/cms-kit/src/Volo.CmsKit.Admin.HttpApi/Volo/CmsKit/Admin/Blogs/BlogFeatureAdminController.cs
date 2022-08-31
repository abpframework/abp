﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.Features;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.Blogs;

[RequiresFeature(CmsKitFeatures.BlogEnable)]
[RequiresGlobalFeature(typeof(BlogsFeature))]
[RemoteService(Name = CmsKitAdminRemoteServiceConsts.RemoteServiceName)]
[Area(CmsKitAdminRemoteServiceConsts.ModuleName)]
[Authorize(CmsKitAdminPermissions.Blogs.Features)]
[Route("api/cms-kit-admin/blogs/{blogId}/features")]
public class BlogFeatureAdminController : CmsKitAdminController, IBlogFeatureAdminAppService
{
    protected IBlogFeatureAdminAppService BlogFeatureAdminAppService { get; }

    public BlogFeatureAdminController(IBlogFeatureAdminAppService blogFeatureAdminAppService)
    {
        BlogFeatureAdminAppService = blogFeatureAdminAppService;
    }

    [HttpGet]
    public Task<List<BlogFeatureDto>> GetListAsync(Guid blogId)
    {
        return BlogFeatureAdminAppService.GetListAsync(blogId);
    }

    [HttpPut]
    public Task SetAsync(Guid blogId, BlogFeatureInputDto dto)
    {
        return BlogFeatureAdminAppService.SetAsync(blogId, dto);
    }
}
