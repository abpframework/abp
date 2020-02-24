using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Docs.GitHub.Documents;
using Volo.Docs.Projects;

namespace Volo.Docs.Documents
{
    public interface IDocumentSource : IDomainService
    {
        Task<Document> GetDocumentAsync(Project project, string documentName, string languageCode, string version);

        Task<List<VersionInfo>> GetVersionsAsync(Project project);

        Task<DocumentResource> GetResource(Project project, string resourceName, string languageCode, string version);

        Task<LanguageConfig> GetLanguageListAsync(Project project, string version);
    }
}