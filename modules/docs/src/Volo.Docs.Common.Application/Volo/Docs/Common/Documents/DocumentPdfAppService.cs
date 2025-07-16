using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Volo.Abp.Content;
using Volo.Abp.Http;
using Volo.Docs.Projects;
using Volo.Docs.Projects.Pdf;

namespace Volo.Docs.Common.Documents;

[Authorize(DocsCommonPermissions.Projects.PdfDownload)]
public class DocumentPdfAppService : DocsCommonAppServiceBase, IDocumentPdfAppService
{
    protected IProjectPdfGenerator ProjectPdfGenerator { get; }
    protected IProjectRepository ProjectRepository { get; }
    protected IProjectPdfFileStore ProjectPdfFileStore { get; }
    protected IOptions<DocsProjectPdfGeneratorOptions> Options { get; } 
    
    public DocumentPdfAppService(
        IProjectPdfGenerator projectPdfGenerator,
        IProjectRepository projectRepository, 
        IProjectPdfFileStore projectPdfFileStore,
        IOptions<DocsProjectPdfGeneratorOptions> options)
    {
        ProjectPdfGenerator = projectPdfGenerator;
        ProjectRepository = projectRepository;
        ProjectPdfFileStore = projectPdfFileStore;
        Options = options;
    }

    public virtual async Task<IRemoteStreamContent> DownloadPdfAsync(DocumentPdfGeneratorInput input)
    {
        var project = await ProjectRepository.GetAsync(input.ProjectId);
        var version = project.GetFullVersion(input.Version);
        var languageCode = input.LanguageCode;
        var fileName = Options.Value.CalculatePdfFileName(project, version, languageCode);
        var fileStream = await ProjectPdfFileStore.GetOrNullAsync(project, version, languageCode);
        
        if (fileStream != null)
        {
            return new RemoteStreamContent(fileStream, fileName, MimeTypes.Application.Zip);
        }

        return null;
    }

    public virtual async Task<bool> ExistsAsync(DocumentPdfGeneratorInput input)
    {
        var project = await ProjectRepository.GetAsync(input.ProjectId);
        var version = project.GetFullVersion(input.Version);
        var languageCode = input.LanguageCode;
        var fileName = Options.Value.CalculatePdfFileName(project, version, languageCode);
        return project.FindPdfFile(fileName) != null;
    }
}