using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.IO;
using Volo.Docs.Documents;
using Volo.Docs.FileSystem.Projects;
using Volo.Docs.Projects;

namespace Volo.Docs.FileSystem.Documents
{
    public class FileSystemDocumentStore : DomainService, IDocumentStore
    {
        public const string Type = "FileSystem";

        public async Task<Document> GetDocumentAsync(Project project, string documentName, string version)
        {
            var projectFolder = project.GetFileSystemPath();
            var path = Path.Combine(projectFolder, documentName);

            CheckDirectorySecurity(projectFolder, path);
            
            var content = await FileHelper.ReadAllTextAsync(path);
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

        public Task<List<VersionInfo>> GetVersionsAsync(Project project)
        {
            return Task.FromResult(new List<VersionInfo>());
        }

        public async Task<DocumentResource> GetResource(Project project, string resourceName, string version)
        {
            var projectFolder = project.GetFileSystemPath();
            var path = Path.Combine(projectFolder, resourceName);

            if (!DirectoryHelper.IsSubDirectoryOf(projectFolder, path))
            {
                throw new SecurityException("Can not get a resource file out of the project folder!");
            }

            return new DocumentResource(await FileHelper.ReadAllBytesAsync(path));
        }

        private static void CheckDirectorySecurity(string projectFolder, string path)
        {
            if (!DirectoryHelper.IsSubDirectoryOf(projectFolder, path))
            {
                throw new SecurityException("Can not get a resource file out of the project folder!");
            }
        }
    }
}
