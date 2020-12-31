using Microsoft.AspNetCore.Authorization;
using System;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.CmsKit.Contents;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.Contents
{
    [Authorize(CmsKitAdminPermissions.Contents.Default)]
    public class ContentAdminAppService : 
        CrudAppService<
            Content,
            ContentDto,
            Guid,
            ContentGetListInput,
            ContentCreateDto,
            ContentUpdateDto
            >, IContentAdminAppService
    {
        public ContentAdminAppService(
            IRepository<Content, Guid> repository,
            IContentRepository contentRepository) : base(repository)
        {
            ContentRepository = contentRepository;

            GetListPolicyName = CmsKitAdminPermissions.Contents.Default;
            GetPolicyName = CmsKitAdminPermissions.Contents.Default;
            CreatePolicyName = CmsKitAdminPermissions.Contents.Create;
            UpdatePolicyName = CmsKitAdminPermissions.Contents.Update;
            DeletePolicyName = CmsKitAdminPermissions.Contents.Delete;
        }

        protected IContentRepository ContentRepository { get; }
    }
}
