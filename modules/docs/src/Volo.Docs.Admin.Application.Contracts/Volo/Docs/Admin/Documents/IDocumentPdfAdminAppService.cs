using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Docs.Admin.Projects;
using Volo.Docs.Common.Documents;

namespace Volo.Docs.Admin.Documents;

public interface IDocumentPdfAdminAppService : IDocumentPdfAppService
{
    Task GeneratePdfAsync(DocumentPdfGeneratorInput input);
    
    Task<PagedResultDto<ProjectPdfFileDto>> GetPdfFilesAsync(GetPdfFilesInput input);

    Task DeletePdfFileAsync(DeletePdfFileInput input);
}