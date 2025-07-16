using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Content;
using Volo.Docs.Common;
using Volo.Docs.Common.Documents;

namespace Volo.Docs.Documents;

[RemoteService(Name = DocsCommonRemoteServiceConsts.RemoteServiceName)]
[Area(DocsCommonRemoteServiceConsts.ModuleName)]
[ControllerName("DocumentPdf")]
[Route("api/docs/documents/pdf")]
public class DocsDocumentPdfController : DocsControllerBase, IDocumentPdfAppService
{
    protected IDocumentPdfAppService DocumentPdfAppService { get; }
    
    public DocsDocumentPdfController(IDocumentPdfAppService documentPdfAppService)
    {
        DocumentPdfAppService = documentPdfAppService;
    }

    [HttpGet]
    [Route("download")]
    public virtual Task<IRemoteStreamContent> DownloadPdfAsync(DocumentPdfGeneratorInput input)
    {
        return DocumentPdfAppService.DownloadPdfAsync(input);
    }

    [HttpGet]
    [Route("exists")]
    public virtual Task<bool> ExistsAsync(DocumentPdfGeneratorInput input)
    {
        return DocumentPdfAppService.ExistsAsync(input);
    }
}