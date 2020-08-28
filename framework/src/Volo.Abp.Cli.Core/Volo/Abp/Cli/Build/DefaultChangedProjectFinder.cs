using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using LibGit2Sharp;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Build
{
    public class DefaultChangedProjectFinder : IChangedProjectFinder, ITransientDependency
    {
        private readonly IRepositoryBuildStatusStore _repositoryBuildStatusStore;

        public DefaultChangedProjectFinder(IRepositoryBuildStatusStore repositoryBuildStatusStore)
        {
            _repositoryBuildStatusStore = repositoryBuildStatusStore;
        }

        public List<DotNetProjectInfo> Find(DotNetProjectBuildConfig buildConfig)
        {
            if (buildConfig.SlFilePath.IsNullOrEmpty())
            {
                return FindByRepository(buildConfig.GitRepository);
            }

            return FindBySlnFile(buildConfig.GitRepository, buildConfig.SlFilePath);
        }
        
        private List<DotNetProjectInfo> FindByRepository(GitRepository gitRepository)
        {
            var changedProjectList = new List<DotNetProjectInfo>();
            var gitRepositoryBuildStatus = _repositoryBuildStatusStore.Get(gitRepository);

            FindChangedFiles(gitRepository, gitRepositoryBuildStatus, changedProjectList);

            return changedProjectList;
        }

        private List<DotNetProjectInfo> FindBySlnFile(GitRepository gitRepository, string slnFilePath)
        {
            var slFine = new FileInfo(slnFilePath);
            var changedProjectList = new List<DotNetProjectInfo>();
            var csProjFiles = slFine.Directory.GetFiles(
                    "*.csproj",
                    SearchOption.AllDirectories
                ).Select(e => e.FullName)
                .ToList();

            foreach (var csProjFile in csProjFiles)
            {
                AddDependingProjectsToList(gitRepository, csProjFile, changedProjectList);
            }

            return changedProjectList;
        }

        private void AddDependingProjectsToList(
            GitRepository gitRepository,
            string csProjFilePath,
            List<DotNetProjectInfo> changedProjectList)
        {
            var repositoryName = gitRepository.FindRepositoryOf(csProjFilePath);
            var project = new DotNetProjectInfo(repositoryName, csProjFilePath);
            if (changedProjectList.Any(e => e.CsProjPath == csProjFilePath))
            {
                return;
            }

            changedProjectList.Add(project);

            AddProjectDependencies(gitRepository, project, changedProjectList);
        }

        private void AddProjectDependencies(GitRepository gitRepository, DotNetProjectInfo project, List<DotNetProjectInfo> changedProjectList)
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

                if (changedProjectList.Any(e => e.CsProjPath == referenceProjectInfo.FullName))
                {
                    continue;
                }
                
                var repositoryName = gitRepository.FindRepositoryOf(referenceProjectInfo.FullName);
                var referenceProject = new DotNetProjectInfo(repositoryName, referenceProjectInfo.FullName);
                changedProjectList.Add(referenceProject);

                AddProjectDependencies(gitRepository, referenceProject, changedProjectList);
            }
        }

        private void FindChangedFiles(
            GitRepository repository,
            GitRepositoryBuildStatus repositoryBuildStatus,
            List<DotNetProjectInfo> changedProjectList)
        {
            if (repositoryBuildStatus.CommitId.IsNullOrEmpty())
            {
                AddAllCsProjFiles(repository, changedProjectList);
            }
            else
            {
                AddChangedCsProjFiles(repository, changedProjectList, repositoryBuildStatus);
            }

            if (repository.DependingRepositories.Any())
            {
                foreach (var dependingRepository in repository.DependingRepositories)
                {
                    var dependingRepositoryBuildStatus = repositoryBuildStatus.GetChild(dependingRepository.Name);
                    FindChangedFiles(dependingRepository, dependingRepositoryBuildStatus, changedProjectList);
                }
            }
        }

        private void AddAllCsProjFiles(GitRepository repository, List<DotNetProjectInfo> changedFiles)
        {
            var allCsProjFiles = Directory.GetFiles(
                repository.RootPath,
                "*.csproj",
                SearchOption.AllDirectories
            );

            foreach (var file in allCsProjFiles)
            {
                changedFiles.Add(
                    new DotNetProjectInfo(repository.Name, Path.Combine(repository.RootPath, file))
                );
            }
        }

        private void AddChangedCsProjFiles(
            GitRepository repository,
            List<DotNetProjectInfo> changedFiles,
            GitRepositoryBuildStatus status)
        {
            using (var repo = new Repository(string.Concat(repository.RootPath, @"\.git")))
            {
                var firstCommit = status.CommitId.IsNullOrEmpty()
                    ? null
                    : repo.Lookup<Commit>(status.CommitId);
                var repoDifferences = repo.Diff.Compare<Patch>(firstCommit?.Tree, repo.Head.Tip.Tree);

                var files = repoDifferences
                    .Where(e => e.Path.EndsWith(".cs") || e.Path.EndsWith(".csproj"))
                    .Where(e => e.Status != ChangeKind.Deleted)
                    .Select(e => e)
                    .ToList();

                foreach (var file in files)
                {
                    var csProjPath = Path.Combine(repository.RootPath, file.Path);
                    if (status.SucceedProjects.Any(p => p.CsProjPath == csProjPath && p.CommitId == "1"))
                    {
                        continue;
                    }

                    changedFiles.Add(
                        new DotNetProjectInfo(repository.Name, csProjPath)
                    );
                }

                if (!repository.DependingRepositories.Any())
                {
                    return;
                }

                foreach (var subRepository in repository.DependingRepositories)
                {
                    var subRepositoryBuildStatus = status.GetChild(subRepository.Name);
                    FindChangedFiles(subRepository, subRepositoryBuildStatus, changedFiles);
                }
            }
        }
    }
}
