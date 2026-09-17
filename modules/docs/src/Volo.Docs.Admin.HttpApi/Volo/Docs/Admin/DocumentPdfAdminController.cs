using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;
using Volo.Docs.Admin.Documents;
using Volo.Docs.Admin.Projects;
using Volo.Docs.Common.Documents;

namespace Volo.Docs.Admin;
[RemoteService(Name = DocsAdminRemoteServiceConsts.RemoteServiceName)]
[Area(DocsAdminRemoteServiceConsts.ModuleName)]
[ControllerName("DocumentsPdfAdmin")]
[Route("api/docs/admin/documents/pdf")]
public class DocumentPdfAdminController :  AbpControllerBase, IDocumentPdfAdminAppService
{
    private readonly IDocumentPdfAdminAppService _documentPdfAdminAppService;

    public DocumentPdfAdminController(IDocumentPdfAdminAppService documentPdfAdminAppService)
    {
        _documentPdfAdminAppService = documentPdfAdminAppService;
    }

    [HttpGet]
    [Route("generate")]
    public Task GeneratePdfAsync(DocumentPdfGeneratorInput input)
    {
        return _documentPdfAdminAppService.GeneratePdfAsync(input);
    }

    [HttpGet]
    [Route("files")]
    public Task<PagedResultDto<ProjectPdfFileDto>> GetPdfFilesAsync(GetPdfFilesInput input)
    {
        return _documentPdfAdminAppService.GetPdfFilesAsync(input);
    }

    [HttpDelete]
    [Route("delete-file")]
    public Task DeletePdfFileAsync(DeletePdfFileInput input)
    {
        return _documentPdfAdminAppService.DeletePdfFileAsync(input);
    }

    [HttpGet]
    [Route("download")]
    public Task<IRemoteStreamContent> DownloadPdfAsync(DocumentPdfGeneratorInput input)
    {
        return _documentPdfAdminAppService.DownloadPdfAsync(input);
    }

    [HttpGet]
    [Route("exists")]
    public Task<bool> ExistsAsync(DocumentPdfGeneratorInput input)
    {
        return _documentPdfAdminAppService.ExistsAsync(input);
    }
}