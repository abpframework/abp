using Volo.Docs.Documents;
using Volo.Docs.Projects;

namespace Volo.Docs.HtmlConverting
{
    public interface IDocumentToHtmlConverter
    {
        string Convert(ProjectDto project, DocumentWithDetailsDto document, string version, string languageCode);
    }
}