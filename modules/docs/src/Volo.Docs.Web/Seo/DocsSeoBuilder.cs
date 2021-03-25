using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Docs.Documents;
using Volo.Docs.Projects;
using Volo.Docs.Utils;

namespace Volo.Docs.Seo
{
    public class DocsSeoBuilder : ITransientDependency
    {
        private readonly DocsUiOptions _uiOptions;
        private readonly IProjectAppService _projectAppService;
        private readonly IDocumentAppService _documentAppService;

        public DocsSeoBuilder(
            IOptions<DocsUiOptions> uiOptions, 
            IProjectAppService projectAppService, 
            IDocumentAppService documentAppService
        )
        {
            _uiOptions = uiOptions.Value;
            _projectAppService = projectAppService;
            _documentAppService = documentAppService;
        }
        
        public async Task<List<string>> GetDocumentLinksAsync()
        {
            var documentLinks = new List<string>();
            
            var projects = await _projectAppService.GetListAsync();

            foreach (var project in projects.Items)
            {
                var documents = await _documentAppService.GetListByProjectId(project.Id);
                
                if (!documents.Any())
                {
                    continue;
                }

                foreach (var document in documents)
                {
                    var rootNode = await _documentAppService.GetNavigationAsync(new GetNavigationDocumentInput
                    {
                        ProjectId = project.Id,
                        LanguageCode = document.LanguageCode,
                        Version = document.Version
                    });

                    rootNode.Items?.ForEach(childNode =>
                    {
                        var link = GetNodeLink(childNode, document.Format, document.LanguageCode, project.ShortName, document.Version);
                        if (!string.IsNullOrWhiteSpace(link))
                        {
                            documentLinks.AddIfNotContains(link);
                        }
                    });
                }
            }

            return documentLinks;
        }
        
         private string GetNodeLink(NavigationNode node, string format, string languageCode, string projectName, string version)
        {
            var content = "";

            node.Items?.ForEach(innerNode =>
            {
                content += GetNodeLink(innerNode, format, languageCode, projectName, version);
            });

            return node.IsEmpty ? content : GetLeafNode(node, format, languageCode, projectName, version);
        }

        private string GetLeafNode(NavigationNode node, string format, string languageCode, string projectName, string version)
        {
            string link = "";
            
            if (node.Path.IsNullOrEmpty() && node.IsLeaf)
            {
                link = node.Text.IsNullOrEmpty() ? "?" : node.Text;
            }
            else
            {
                link = NormalizePath(node.Path, format, languageCode, projectName, version);
            }

            return link;
        }

        private string NormalizePath(string path, string format, string languageCode, string projectName, string version)
        {
            if (UrlHelper.IsExternalLink(path) || string.IsNullOrWhiteSpace(path))
            {
                return path;
            }

            var pathWithoutFileExtension = RemoveFileExtensionFromPath(path, format);

            var prefix = _uiOptions.RoutePrefix;

            return prefix + languageCode + "/" + projectName + "/" + version + "/" + pathWithoutFileExtension;
        }

        private string RemoveFileExtensionFromPath(string path, string format)
        {
            if (path == null)
            {
                return null;
            }

            return path.EndsWith("." + format)
                 ? path.Left(path.Length - format.Length - 1)
                 : path;
        }
    }
}