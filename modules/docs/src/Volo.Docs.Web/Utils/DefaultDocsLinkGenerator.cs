using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Docs.Pages.Documents.Project;

namespace Volo.Docs.Utils;

public class DefaultDocsLinkGenerator : IDocsLinkGenerator, ITransientDependency
{
    protected LinkGenerator LinkGenerator { get; }
    
    protected IOptions<DocsUiOptions> DocsUiOptions { get; }

    public DefaultDocsLinkGenerator(LinkGenerator linkGenerator, IOptions<DocsUiOptions> docsUiOptions)
    {
        LinkGenerator = linkGenerator;
        DocsUiOptions = docsUiOptions;
    }


    public string GenerateLink(string projectName, string languageCode, string version, string documentName)
    {
        var routeValues = new Dictionary<string, object> {
            { nameof(IndexModel.LanguageCode), languageCode },
            { nameof(IndexModel.Version), version },
            { nameof(IndexModel.DocumentName), documentName },
            { nameof(IndexModel.ProjectName), projectName }
        };

        var encodedUrl = LinkGenerator.GetPathByPage("/Documents/Project/Index", values: routeValues);
        var url = encodedUrl?.Replace("%2F", "/"); //Document name can contain path separator(/), so we need to decode it.
        return DocsUiOptions.Value.DocumentLinksNormalizer?.Invoke(url);
    }
}