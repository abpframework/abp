using System.Threading.Tasks;

namespace Volo.Docs.Projects.Pdf;

public interface IProjectPdfGenerator
{
    Task GenerateAsync(Project project, string version, string languageCode);
}