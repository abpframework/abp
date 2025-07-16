using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Volo.Abp.Application.Dtos;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DistributedLocking;
using Volo.Docs.Admin.BackgroundJobs;
using Volo.Docs.Admin.Projects;
using Volo.Docs.Common.Documents;
using Volo.Docs.Projects;
using Volo.Docs.Projects.Pdf;

namespace Volo.Docs.Admin.Documents;

[Authorize(DocsAdminPermissions.Projects.ManagePdfFiles)]
public class DocumentPdfAdminAppService : DocumentPdfAppService, IDocumentPdfAdminAppService
{
    protected IBackgroundJobManager BackgroundJobManager { get; }
    protected IAbpDistributedLock DistributedLock { get; }

    public DocumentPdfAdminAppService(
        IProjectPdfGenerator projectPdfGenerator,
        IProjectRepository projectRepository, 
        IProjectPdfFileStore projectPdfFileStore,
        IOptions<DocsProjectPdfGeneratorOptions> options, 
        IBackgroundJobManager backgroundJobManager, 
        IAbpDistributedLock distributedLock) :
        base(projectPdfGenerator, projectRepository, projectPdfFileStore, options)
    {
        BackgroundJobManager = backgroundJobManager;
        DistributedLock = distributedLock;
    }

    public virtual async Task GeneratePdfAsync(DocumentPdfGeneratorInput input)
    {
        var project = await ProjectRepository.GetAsync(input.ProjectId, includeDetails: true);
        await BackgroundJobManager.EnqueueAsync(new DocumentPdfGenerateJobArgs
        {
            Version = project.GetFullVersion(input.Version),
            LanguageCode = input.LanguageCode,
            ProjectId = input.ProjectId,
        });
    }
    
    public virtual async Task<PagedResultDto<ProjectPdfFileDto>> GetPdfFilesAsync(GetPdfFilesInput input)
    {
        var project = await ProjectRepository.GetAsync(input.ProjectId, includeDetails: true);

        var pdfFiles = project.PdfFiles.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

        return new PagedResultDto<ProjectPdfFileDto>(
            project.PdfFiles.Count,
            ObjectMapper.Map<List<ProjectPdfFile>, List<ProjectPdfFileDto>>(pdfFiles)
        );
    }

    public virtual async Task DeletePdfFileAsync(DeletePdfFileInput input)
    {
        var project = await ProjectRepository.GetAsync(input.ProjectId, includeDetails: true);
        await ProjectPdfFileStore.DeleteAsync(project, input.Version, input.LanguageCode);
    }
}