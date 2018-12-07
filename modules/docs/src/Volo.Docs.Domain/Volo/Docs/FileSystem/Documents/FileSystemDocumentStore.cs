using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Docs.Documents;
using Volo.Docs.FileSystem.Projects;
using Volo.Docs.Projects;

namespace Volo.Docs.FileSystem.Documents
{
    public class FileSystemDocumentStore : DomainService, IDocumentStore
    {
        public const string Type = "FileSystem";

        public async Task<Document> GetDocument(Project project, string documentName, string version)
        {
            var path = Path.Combine(project.GetFileSystemPath(), documentName);
            var content = File.ReadAllText(path); //TODO: async!
            var localDirectory = "";

            if (documentName.Contains("/"))
            {
                localDirectory = documentName.Substring(0, documentName.LastIndexOf('/'));
            }

            return new Document
            {
                Content = content,
                FileName = Path.GetFileName(path),
                Format = project.Format,
                LocalDirectory = localDirectory,
                Title = documentName,
                RawRootUrl = $"/document-resources?projectId={project.Id.ToString()}&version={version}&name=",
                RootUrl = "/"
            };
        }

        public Task<List<VersionInfo>> GetVersions(Project project)
        {
            return Task.FromResult(new List<VersionInfo>());
        }

        public async Task<DocumentResource> GetResource(Project project, string resourceName, string version)
        {
            var path = Path.Combine(project.GetFileSystemPath(), resourceName);
            return new DocumentResource(File.ReadAllBytes(path));
        }
    }
}
