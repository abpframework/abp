using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Docs.Documents
{
    [RemoteService(Name = DocsRemoteServiceConsts.RemoteServiceName)]
    [Area(DocsRemoteServiceConsts.ModuleName)]
    [ControllerName("Document")]
    [Route("api/docs/documents")]
    public class DocsDocumentController :  AbpController, IDocumentAppService
    {
        protected IDocumentAppService DocumentAppService { get; }

        public DocsDocumentController(IDocumentAppService documentAppService)
        {
            DocumentAppService = documentAppService;
        }

        [HttpGet]
        [Route("")]
        public virtual Task<DocumentWithDetailsDto> GetAsync(GetDocumentInput input)
        {
            return DocumentAppService.GetAsync(input);
        }

        [HttpGet]
        [Route("default")]
        public virtual Task<DocumentWithDetailsDto> GetDefaultAsync(GetDefaultDocumentInput input)
        {
            return DocumentAppService.GetDefaultAsync(input);
        }

        [HttpGet]
        [Route("navigation")]
        public Task<NavigationNode> GetNavigationAsync(GetNavigationDocumentInput input)
        {
            return DocumentAppService.GetNavigationAsync(input);
        }

        [HttpGet]
        [Route("resource")]
        public Task<DocumentResourceDto> GetResourceAsync(GetDocumentResourceInput input)
        {
            return DocumentAppService.GetResourceAsync(input);
        }

        [HttpPost]
        [Route("search")]
        public Task<List<DocumentSearchOutput>> SearchAsync(DocumentSearchInput input)
        {
            return DocumentAppService.SearchAsync(input);
        }

        [HttpGet]
        [Route("full-search-enabled")]
        public Task<bool> FullSearchEnabledAsync()
        {
            return DocumentAppService.FullSearchEnabledAsync();
        }

        [HttpGet]
        [Route("links")]
        public Task<List<string>> GetUrlsAsync(string prefix)
        {
            return DocumentAppService.GetUrlsAsync(prefix);
        }

        [HttpGet]
        [Route("parameters")]
        public Task<DocumentParametersDto> GetParametersAsync(GetParametersDocumentInput input)
        {
            return DocumentAppService.GetParametersAsync(input);
        }
    }
}
