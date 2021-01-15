using System;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.CmsKit.Domain.Volo.CmsKit.Blogs;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.Blogs
{
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
    }
}
