using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.BlobStoring;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Timing;

namespace Volo.Docs.Projects.Pdf;

public class BlobProjectPdfFileStore : IProjectPdfFileStore, ITransientDependency
{
    protected IBlobContainer<DocsProjectPdfContainer> BlobContainer { get; }
    protected IOptions<DocsProjectPdfGeneratorOptions> Options { get; }
    protected IProjectRepository ProjectRepository { get; }
    protected IClock Clock { get; }
    
    public BlobProjectPdfFileStore(
        IBlobContainer<DocsProjectPdfContainer> blobContainer, 
        IOptions<DocsProjectPdfGeneratorOptions> options, 
        IClock clock, IProjectRepository projectRepository)
    {
        BlobContainer = blobContainer;
        Options = options;
        Clock = clock;
        ProjectRepository = projectRepository;
    }
    
    public virtual async Task SetAsync(Project project, string version, string languageCode, Stream stream)
    {
        var fileName = Options.Value.CalculatePdfFileName(project, version, languageCode);
        await BlobContainer.SaveAsync(fileName, stream, true);
        
        var pdfFile = project.FindPdfFile(fileName);
        if(pdfFile == null)
        {
            project.AddPdfFile(project.Id, fileName, version, languageCode);
        }
        else
        {
            pdfFile.LastModificationTime = Clock.Now;
        }

        await ProjectRepository.UpdateAsync(project);
    }

    public virtual async Task<Stream> GetOrNullAsync(Project project, string version, string languageCode)
    {
        var fileName = Options.Value.CalculatePdfFileName(project, version, languageCode);
        
        var pdfFile = project.FindPdfFile(fileName);
        if (pdfFile == null)
        {
            return null;
        }
        
        return await BlobContainer.GetOrNullAsync(Options.Value.CalculatePdfFileName(project, version, languageCode));
    }

    public virtual async Task DeleteAsync(Project project, string version, string languageCode)
    {
        var fileName = Options.Value.CalculatePdfFileName(project, version, languageCode);
        
        await BlobContainer.DeleteAsync(fileName);
        project.RemovePdfFile(fileName);
        await ProjectRepository.UpdateAsync(project);
        
    }
}