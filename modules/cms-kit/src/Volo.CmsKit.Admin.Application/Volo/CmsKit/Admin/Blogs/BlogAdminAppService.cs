using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.Blogs
{
    [RequiresGlobalFeature(typeof(BlogsFeature))]
    [Authorize(CmsKitAdminPermissions.Blogs.Default)]
    public class BlogAdminAppService : CmsKitAdminAppServiceBase, IBlogAdminAppService
    {
        protected IBlogRepository BlogRepository { get; }
        protected BlogManager BlogManager { get; }
        protected BlogFeatureManager BlogFeatureManager{ get; }

        public BlogAdminAppService(
            IBlogRepository blogRepository, 
            BlogManager blogManager, 
            BlogFeatureManager blogFeatureManager = null)
        {
            BlogRepository = blogRepository;
            BlogManager = blogManager;
            BlogFeatureManager = blogFeatureManager;
        }

        public virtual async Task<BlogDto> GetAsync(Guid id)
        {
            var blog = await BlogRepository.GetAsync(id);

            return ObjectMapper.Map<Blog, BlogDto>(blog);
        }

        public virtual async Task<PagedResultDto<BlogDto>> GetListAsync(BlogGetListInput input)
        {
            var totalCount = await BlogRepository.GetCountAsync(input.Filter);

            var blogs = await BlogRepository.GetListAsync(
                input.Filter,
                input.Sorting,
                input.MaxResultCount,
                input.SkipCount);

            return new PagedResultDto<BlogDto>(totalCount, ObjectMapper.Map<List<Blog>, List<BlogDto>>(blogs));
        }

        [Authorize(CmsKitAdminPermissions.Blogs.Create)]
        public virtual async Task<BlogDto> CreateAsync(CreateBlogDto input)
        {
            var blog = await BlogManager.CreateAsync(input.Name, input.Slug);

            await BlogRepository.InsertAsync(blog, autoSave: true);

            await BlogFeatureManager.SetDefaultsAsync(blog.Id);

            return ObjectMapper.Map<Blog, BlogDto>(blog);
        }

        [Authorize(CmsKitAdminPermissions.Blogs.Update)]
        public virtual async Task<BlogDto> UpdateAsync(Guid id, UpdateBlogDto input)
        {
            var blog = await BlogRepository.GetAsync(id);

            blog = await BlogManager.UpdateAsync(blog, input.Name, input.Slug);

            blog.SetConcurrencyStamp(input.ConcurrencyStamp);

            await BlogRepository.UpdateAsync(blog);

            return ObjectMapper.Map<Blog, BlogDto>(blog);
        }

        [Authorize(CmsKitAdminPermissions.Blogs.Delete)]
        public virtual Task DeleteAsync(Guid id)
        {
            return BlogRepository.DeleteAsync(id);
        }
    }
}
