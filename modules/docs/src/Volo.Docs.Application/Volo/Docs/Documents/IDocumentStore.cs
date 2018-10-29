using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Volo.Docs.Documents
{
    public interface IDocumentStore : IDomainService
    {
        Task<Document> FindDocumentByNameAsync(Dictionary<string, object> projectExtraProperties, string projectFormat,
            string documentName, string version);

        Task<List<VersionInfoDto>> GetVersions(Dictionary<string, object> projectExtraProperties, string documentName);
    }
}