using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.Blogs
{
    [RequiresGlobalFeature(typeof(BlogsFeature))]
    [Authorize(CmsKitAdminPermissions.Blogs.Default)]
    public class BlogAdminAppService : CrudAppService<Blog, BlogDto, Guid, BlogGetListInput>, IBlogAdminAppService
    {
        protected readonly IBlogManager BlogManager;
        
        public BlogAdminAppService(
            IRepository<Blog, Guid> repository,
            IBlogManager blogManager) : base(repository)
        {
            BlogManager = blogManager;
            GetListPolicyName = CmsKitAdminPermissions.Blogs.Default;
            GetPolicyName = CmsKitAdminPermissions.Blogs.Default;
            CreatePolicyName = CmsKitAdminPermissions.Blogs.Create;
            UpdatePolicyName = CmsKitAdminPermissions.Blogs.Update;
        }

        protected override async Task<IQueryable<Blog>> CreateFilteredQueryAsync(BlogGetListInput input)
        {
            var queryable = await base.CreateFilteredQueryAsync(input);
            return queryable
                    .WhereIf(
                        !input.Filter.IsNullOrWhiteSpace(),
                        x => x.Name.ToLower().Contains(input.Filter));
        }

        [Authorize(CmsKitAdminPermissions.Blogs.Delete)]
        public override async Task DeleteAsync(Guid id)
        {            
            await BlogManager.DeleteAsync(id);
        }
    }
}
