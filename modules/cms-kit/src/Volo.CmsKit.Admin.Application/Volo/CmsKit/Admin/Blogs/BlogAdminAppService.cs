using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.ObjectExtending;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.Features;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.Blogs;

[RequiresFeature(CmsKitFeatures.BlogEnable)]
[RequiresGlobalFeature(typeof(BlogsFeature))]
[Authorize(CmsKitAdminPermissions.Blogs.Default)]
public class BlogAdminAppService : CmsKitAdminAppServiceBase, IBlogAdminAppService
{
    protected IBlogRepository BlogRepository { get; }
    protected IBlogPostRepository BlogPostRepository { get; }
    protected BlogManager BlogManager { get; }
    protected BlogFeatureManager BlogFeatureManager { get; }

    public BlogAdminAppService(
        IBlogRepository blogRepository,
        BlogManager blogManager,
        IBlogPostRepository blogPostRepository,
        BlogFeatureManager blogFeatureManager = null)
    {
        BlogRepository = blogRepository;
        BlogManager = blogManager;
        BlogPostRepository = blogPostRepository;
        BlogFeatureManager = blogFeatureManager;
    }

    public virtual async Task<BlogDto> GetAsync(Guid id)
    {
        var blog = await BlogRepository.GetAsync(id);

        var blogDto = ObjectMapper.Map<Blog, BlogDto>(blog);
        blogDto.BlogPostCount = await BlogPostRepository.GetCountAsync(blogId : blog.Id);

        return blogDto;
    }

    public virtual async Task<PagedResultDto<BlogDto>> GetListAsync(BlogGetListInput input)
    {
        var totalCount = await BlogRepository.GetCountAsync(input.Filter);

        var blogs = await BlogRepository.GetListWithBlogPostCountAsync(
            input.Filter,
            input.Sorting,
            input.MaxResultCount,
            input.SkipCount);

        var blogDtos = new PagedResultDto<BlogDto>(totalCount, ObjectMapper.Map<List<Blog>, List<BlogDto>>(blogs.Select(x => x.Blog).ToList()));

        foreach (var blogDto in blogDtos.Items)
        {
            blogDto.BlogPostCount = blogs.First(x => x.Blog.Id == blogDto.Id).BlogPostCount;
        }

        return blogDtos;
    }

    public virtual async Task<ListResultDto<BlogDto>> GetAllListAsync()
    {
        var blogs = await BlogRepository.GetListWithBlogPostCountAsync(maxResultCount: int.MaxValue);

        var blogDtos = new ListResultDto<BlogDto>(ObjectMapper.Map<List<Blog>, List<BlogDto>>(blogs.Select(x => x.Blog).ToList()));

        foreach (var blogDto in blogDtos.Items)
        {
            blogDto.BlogPostCount = blogs.First(x => x.Blog.Id == blogDto.Id).BlogPostCount;
        }

        return blogDtos;
    }

    [Authorize(CmsKitAdminPermissions.Blogs.Create)]
    public virtual async Task<BlogDto> CreateAsync(CreateBlogDto input)
    {
        var blog = await BlogManager.CreateAsync(input.Name, input.Slug);
        input.MapExtraPropertiesTo(blog);

        await BlogRepository.InsertAsync(blog, autoSave: true);

        await BlogFeatureManager.SetDefaultsAsync(blog.Id);

        return ObjectMapper.Map<Blog, BlogDto>(blog);
    }

    [Authorize(CmsKitAdminPermissions.Blogs.Update)]
    public virtual async Task<BlogDto> UpdateAsync(Guid id, UpdateBlogDto input)
    {
        var blog = await BlogRepository.GetAsync(id);

        blog = await BlogManager.UpdateAsync(blog, input.Name, input.Slug);
        input.MapExtraPropertiesTo(blog);
        blog.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);

        await BlogRepository.UpdateAsync(blog);

        return ObjectMapper.Map<Blog, BlogDto>(blog);
    }

    [Authorize(CmsKitAdminPermissions.Blogs.Delete)]
    public virtual async Task MoveAllBlogPostsAsync(Guid blogId, Guid? assignToBlogId)
    {
        var blog = await BlogRepository.GetAsync(blogId);
        await BlogPostRepository.UpdateBlogAsync(blog.Id, assignToBlogId);
    }

    [Authorize(CmsKitAdminPermissions.Blogs.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await BlogPostRepository.DeleteByBlogIdAsync(id);
        await BlogRepository.DeleteAsync(id);
    }
}
