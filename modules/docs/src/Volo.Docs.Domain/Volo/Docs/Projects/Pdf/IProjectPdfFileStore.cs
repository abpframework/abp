using System.IO;
using System.Threading.Tasks;

namespace Volo.Docs.Projects.Pdf;

public interface IProjectPdfFileStore
{
    Task SetAsync(Project project, string version, string languageCode, Stream stream);
    
    Task<Stream> GetOrNullAsync(Project project, string version, string languageCode);

    Task DeleteAsync(Project project, string version, string languageCode);
}