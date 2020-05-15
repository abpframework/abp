using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Docs.Admin.Documents;

namespace Volo.Docs.Admin
{
    [RemoteService]
    [Area("docs")]
    [ControllerName("DocumentsAdmin")]
    [Route("api/docs/admin/documents")]
    public class DocumentsAdminController : AbpController, IDocumentAdminAppService
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
    }
}
