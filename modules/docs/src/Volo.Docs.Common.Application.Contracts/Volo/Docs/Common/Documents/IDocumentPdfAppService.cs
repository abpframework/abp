using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace Volo.Docs.Common.Documents;

public interface IDocumentPdfAppService : IApplicationService
{
    Task<IRemoteStreamContent> DownloadPdfAsync(DocumentPdfGeneratorInput input);
    
    Task<bool> ExistsAsync(DocumentPdfGeneratorInput input);
}