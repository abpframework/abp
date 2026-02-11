using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Docs.Areas.Models.DocumentNavigation;
using Volo.Docs.Documents;
using Volo.Docs.Utils;

namespace Volo.Docs.Areas.Documents;

[RemoteService(Name = DocsRemoteServiceConsts.RemoteServiceName)]
[Area(DocsRemoteServiceConsts.ModuleName)]
[ControllerName("DocumentNavigation")]
[Route("/docs/document-navigation")]
public class DocumentNavigationController : AbpController
{
    private readonly IDocumentAppService _documentAppService;
    private readonly IDocsLinkGenerator _docsLinkGenerator;

    public DocumentNavigationController(IDocumentAppService documentAppService, IDocsLinkGenerator docsLinkGenerator)
    {
        _documentAppService = documentAppService;
        _docsLinkGenerator = docsLinkGenerator;
    }

    [HttpGet]
    [Route("")]
    public virtual async Task<NavigationNode> GetNavigationAsync(GetNavigationNodeWithLinkModel input)
    {
        var navigationNode = await _documentAppService.GetNavigationAsync(new GetNavigationDocumentInput
        {
            LanguageCode = input.LanguageCode,
            Version = input.Version,
            ProjectId = input.ProjectId
        });
        
        NormalPath(navigationNode, input);
        
        return navigationNode;
    }
    
    protected virtual void NormalPath(NavigationNode node, GetNavigationNodeWithLinkModel input)
    {
        if (node.HasChildItems)
        {
            foreach (var item in node.Items)
            {
                NormalPath(item, input);
            }
        }
        
        if (UrlHelper.IsExternalLink(node.Path))
        {
            return;
        }
        
        node.Path = RemoveFileExtensionFromPath(node.Path, input.ProjectFormat);
        if (node.Path.IsNullOrWhiteSpace())
        {
            node.Path = "javascript:;";
            return;
        }
        
        node.Path = _docsLinkGenerator.GenerateLink(input.ProjectName, input.LanguageCode, input.RouteVersion, node.Path);
    }
    
    private string RemoveFileExtensionFromPath(string path, string projectFormat)
    {
        if (path == null)
        {
            return null;
        }

        return path.EndsWith("." + projectFormat)
            ? path.Left(path.Length - projectFormat.Length - 1)
            : path;
    }
}