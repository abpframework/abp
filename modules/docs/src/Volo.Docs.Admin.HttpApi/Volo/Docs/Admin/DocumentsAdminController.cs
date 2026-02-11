using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Docs.Admin.Documents;
using Volo.Docs.Admin.Projects;

namespace Volo.Docs.Admin
{
    [RemoteService(Name = DocsAdminRemoteServiceConsts.RemoteServiceName)]
    [Area(DocsAdminRemoteServiceConsts.ModuleName)]
    [ControllerName("DocumentsAdmin")]
    [Route("api/docs/admin/documents")]
    public class DocumentsAdminController : AbpControllerBase, IDocumentAdminAppService
    {
        private readonly IDocumentAdminAppService _documentAdminAppService;

        public DocumentsAdminController(IDocumentAdminAppService documentAdminAppService)
        {
            _documentAdminAppService = documentAdminAppService;
        }

        [HttpPost]
        [Route("ClearCache")]
        public Task ClearCacheAsync(ClearCacheInput input)
        {
            return _documentAdminAppService.ClearCacheAsync(input);
        }

        [HttpPost]
        [Route("PullAll")]
        public Task PullAllAsync(PullAllDocumentInput input)
        {
            return _documentAdminAppService.PullAllAsync(input);
        }

        [HttpPost]
        [Route("Pull")]
        public Task PullAsync(PullDocumentInput input)
        {
            return _documentAdminAppService.PullAsync(input);
        }

        [HttpGet]
        [Route("GetAll")]
        public Task<PagedResultDto<DocumentDto>> GetAllAsync(GetAllInput input)
        {
            return _documentAdminAppService.GetAllAsync(input);
        }

        [HttpPut]
        [Route("RemoveDocumentFromCache")]
        public virtual async Task RemoveFromCacheAsync(Guid documentId)
        {
            await _documentAdminAppService.RemoveFromCacheAsync(documentId);
        }

        [HttpPut]
        [Route("ReindexDocument")]
        public virtual async Task ReindexAsync(Guid documentId)
        {
            await _documentAdminAppService.ReindexAsync(documentId);
        }

        [HttpGet]
        [Route("GetFilterItems")]
        public virtual async Task<List<DocumentInfoDto>> GetFilterItemsAsync()
        {
            return await _documentAdminAppService.GetFilterItemsAsync();
        }

        [HttpGet]
        [Route("GetProjects")]
        public virtual Task<List<ProjectWithoutDetailsDto>> GetProjectsAsync()
        {
            return _documentAdminAppService.GetProjectsAsync();
        }
    }
}
