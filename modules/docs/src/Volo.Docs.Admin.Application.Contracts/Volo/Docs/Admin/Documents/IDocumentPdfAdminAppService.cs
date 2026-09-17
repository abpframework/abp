using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using Volo.Docs.Admin.Projects;
using Volo.Docs.Common.Documents;

namespace Volo.Docs.Admin.Documents;

public interface IDocumentPdfAdminAppService : IApplicationService
{
    Task<IRemoteStreamContent> DownloadPdfAsync(DocumentPdfGeneratorInput input);

    Task<bool> ExistsAsync(DocumentPdfGeneratorInput input);

    Task GeneratePdfAsync(DocumentPdfGeneratorInput input);

    Task<PagedResultDto<ProjectPdfFileDto>> GetPdfFilesAsync(GetPdfFilesInput input);

    Task DeletePdfFileAsync(DeletePdfFileInput input);
}
