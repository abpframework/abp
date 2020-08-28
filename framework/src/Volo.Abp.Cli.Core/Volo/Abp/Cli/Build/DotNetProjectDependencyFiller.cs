using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Build
{
    public class DotNetProjectDependencyFiller : IDotNetProjectDependencyFiller, ITransientDependency
    {
        public ILogger<DotNetProjectDependencyFiller> Logger { get; set; }
        
        public void Fill(List<DotNetProjectInfo> projects)
        {
            foreach (var project in projects)
            {
                FillProjectDependencies(project);
            }
        }

        private void FillProjectDependencies(DotNetProjectInfo project)
        {
            var projectNode = XElement.Load(project.CsProjPath);
            var referenceNodes = projectNode.Descendants("ItemGroup").Descendants("ProjectReference");

            foreach (var referenceNode in referenceNodes)
            {
                if (referenceNode.Attribute("Include") == null)
                {
                    continue;
                }

                var relativePath = referenceNode.Attribute("Include").Value;
                var file = new FileInfo(project.CsProjPath);
                var referenceProjectInfo = new FileInfo(Path.Combine(file.Directory.FullName, relativePath));

                var referenceProject = new DotNetProjectInfo(project.RepositoryName, referenceProjectInfo.FullName);

                project.Dependencies.Add(referenceProject);
            }
        }

        private void GetProjectsOfRepository(GitRepository repository, List<string> projectPaths)
        {
            Logger.LogInformation("repo name:" + repository.Name);
            Logger.LogInformation("directoryPath:" + repository.RootPath);
            
            GetProjectsOfADirectory(repository.RootPath, projectPaths, new List<string>());

            if (!repository.DependingRepositories.Any())
            {
                return;
            }

            foreach (var dependantRepository in repository.DependingRepositories)
            {
                GetProjectsOfRepository(dependantRepository, projectPaths);
            }
        }

        private void GetProjectsOfADirectory(
            string directoryPath, 
            List<string> projectPaths,
            List<string> ignoredDirectories)
        {
            Logger.LogInformation("directoryPath:" + directoryPath);
            
            var files = Directory.GetFiles(directoryPath, "*.csproj", SearchOption.AllDirectories)
                .Select(f => f)
                .ToList();

            // foreach (var ignoredDirectory in ignoredDirectories)
            // {
            //     files = files.Where(e => !e.StartsWith(Path.Combine(directoryPath, ignoredDirectory)))
            //         .ToList();
            // }

            projectPaths.AddRange(files);

            // foreach (var projectFile in files)
            // {
            //     var projectNode = XElement.Load(projectFile);
            //     if (projectNode.Attribute("Sdk") == null)
            //     {
            //         continue;
            //     }
            //
            //     projectPaths.Add(projectFile);
            // }
        }
    }
}
