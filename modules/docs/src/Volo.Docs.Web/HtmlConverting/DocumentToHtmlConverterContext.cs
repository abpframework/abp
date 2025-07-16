using Volo.Docs.Common.Projects;
using Volo.Docs.Documents;

namespace Volo.Docs.HtmlConverting;

public class DocumentToHtmlConverterContext
{
    public ProjectDto Project { get; set; }
    public DocumentWithDetailsDto Document { get; set; }
    public string Version { get; set; }
    public string LanguageCode { get; set; }
    public string ProjectShortName { get; set; }
    
    public DocumentToHtmlConverterContext(ProjectDto project,
        DocumentWithDetailsDto document,
        string version,
        string languageCode,
        string projectShortName = null)
    {
        Project = project;
        Document = document;
        Version = version;
        LanguageCode = languageCode;
        ProjectShortName = projectShortName;
    }
}