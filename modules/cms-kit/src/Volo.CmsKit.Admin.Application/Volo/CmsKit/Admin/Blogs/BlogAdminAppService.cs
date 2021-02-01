using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Admin.Application.Contracts.Volo.CmsKit.Admin.Blogs;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.Blogs
{
    [RequiresGlobalFeature(typeof(BlogsFeature))]
    [Authorize(CmsKitAdminPermissions.Blogs.Default)]
    public class BlogAdminAppService : CrudAppService<Blog, BlogDto, Guid>, IBlogAdminAppService
    {
        public BlogAdminAppService(IRepository<Blog, Guid> repository) : base(repository)
        {
            GetListPolicyName = CmsKitAdminPermissions.Blogs.Default;
            GetPolicyName = CmsKitAdminPermissions.Blogs.Default;
            CreatePolicyName = CmsKitAdminPermissions.Blogs.Create;
            UpdatePolicyName = CmsKitAdminPermissions.Blogs.Update;
            DeletePolicyName = CmsKitAdminPermissions.Blogs.Delete;
        }

        [Authorize(CmsKitAdminPermissions.Blogs.Default)]
        public async Task<List<BlogLookupDto>> GetLookupAsync()
        {
            var blogs = await Repository.GetListAsync();

            return blogs
                    .Select(s => new BlogLookupDto(s.Id, s.Name))
                    .ToList();
        }
    }
}
