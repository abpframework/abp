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
        private readonly IGitRepositoryHelper _gitRepositoryHelper;
        private readonly List<string> _changeDetectionFileExtensions = new List<string>
        {
            ".cs",
            ".csproj",
            ".cshtml"
        };

        public DefaultChangedProjectFinder(
            IRepositoryBuildStatusStore repositoryBuildStatusStore,
            IGitRepositoryHelper gitRepositoryHelper)
        {
            _repositoryBuildStatusStore = repositoryBuildStatusStore;
            _gitRepositoryHelper = gitRepositoryHelper;
        }

        public List<DotNetProjectInfo> Find(DotNetProjectBuildConfig buildConfig)
        {
            if (buildConfig.SlFilePath.IsNullOrEmpty())
            {
                return FindByRepository(buildConfig);
            }
        
            return FindBySlnFile(buildConfig.GitRepository, buildConfig.SlFilePath);
        }

        private List<DotNetProjectInfo> FindByRepository(DotNetProjectBuildConfig buildConfig)
        {
            var changedProjectList = new List<DotNetProjectInfo>();
            var gitRepositoryBuildStatus = _repositoryBuildStatusStore.Get(
                buildConfig.BuildName,
                buildConfig.GitRepository
            );

            FindChangedFiles(
                buildConfig.GitRepository,
                gitRepositoryBuildStatus,
                changedProjectList,
                buildConfig.ForceBuild
            );

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

        private void AddProjectDependencies(GitRepository gitRepository, DotNetProjectInfo project,
            List<DotNetProjectInfo> changedProjectList)
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
            List<DotNetProjectInfo> changedProjectList,
            bool forceBuild)
        {
            if (forceBuild || repositoryBuildStatus == null || repositoryBuildStatus.CommitId.IsNullOrEmpty())
            {
                AddAllCsProjFiles(repository, changedProjectList, repositoryBuildStatus);
            }
            else
            {
                AddChangedCsProjFiles(
                    repository,
                    changedProjectList,
                    repositoryBuildStatus,
                    false
                );
            }

            if (repository.DependingRepositories.Any())
            {
                foreach (var dependingRepository in repository.DependingRepositories)
                {
                    var dependingRepositoryBuildStatus = repositoryBuildStatus?.GetChild(dependingRepository.Name);
                    FindChangedFiles(
                        dependingRepository,
                        dependingRepositoryBuildStatus,
                        changedProjectList,
                        forceBuild
                    );
                }
            }
        }

        private void AddAllCsProjFiles(
            GitRepository repository,
            List<DotNetProjectInfo> changedFiles,
            GitRepositoryBuildStatus status)
        {
            var allCsProjFiles = Directory.GetFiles(
                repository.RootPath,
                "*.csproj",
                SearchOption.AllDirectories
            ).ToList();

            // Filter ignored directories
            foreach (var ignoredDirectory in repository.IgnoredDirectories)
            {
                allCsProjFiles = allCsProjFiles.Where(e =>
                        !e.StartsWith(Path.Combine(repository.RootPath, ignoredDirectory)))
                    .ToList();
            }

            // Filter already built files.
            var lastCommitId = _gitRepositoryHelper.GetLastCommitId(repository);

            foreach (var file in allCsProjFiles)
            {
                if (status == null)
                {
                    changedFiles.Add(
                        new DotNetProjectInfo(repository.Name, Path.Combine(repository.RootPath, file))
                    );

                    continue;
                }

                if (status.GetSelfOrChild(repository.Name).SucceedProjects.Any(e => e.CsProjPath == file && e.CommitId == lastCommitId))
                {
                    continue;
                }

                changedFiles.Add(
                    new DotNetProjectInfo(repository.Name, Path.Combine(repository.RootPath, file))
                );
            }
        }
        
        private void AddChangedCsProjFiles(
            GitRepository repository,
            List<DotNetProjectInfo> changedFiles,
            GitRepositoryBuildStatus status,
            bool forceBuild)
        {
            using (var repo = new Repository(string.Concat(repository.RootPath, @"\.git")))
            {
                var firstCommit = status.CommitId.IsNullOrEmpty()
                    ? null
                    : repo.Lookup<Commit>(status.CommitId);
                var repoDifferences = repo.Diff.Compare<Patch>(firstCommit?.Tree, repo.Head.Tip.Tree);
                
                var fileExtensionPredicate = PredicateBuilder.New<PatchEntryChanges>(true);

                foreach (var changeDetectionFileExtension in _changeDetectionFileExtensions)
                {
                    fileExtensionPredicate = fileExtensionPredicate.And(e => e.Path.EndsWith(changeDetectionFileExtension));
                }
                
                var files = repoDifferences
                    .Where(fileExtensionPredicate)
                    .Where(e => e.Status != ChangeKind.Deleted)
                    .Select(e => e)
                    .ToList();

                var affectedCsProjFiles = FindAffectedCsProjFiles(repository.RootPath, files);
                var lastCommitId = _gitRepositoryHelper.GetLastCommitId(repository);
                
                foreach (var file in affectedCsProjFiles)
                {
                    var csProjPath = Path.Combine(repository.RootPath, file);
                    if (status.SucceedProjects.Any(p => p.CsProjPath == csProjPath && p.CommitId == lastCommitId))
                    {
                        continue;
                    }

                    // Filter ignored directories
                    foreach (var ignoredDirectory in repository.IgnoredDirectories)
                    {
                        if (csProjPath.StartsWith(Path.Combine(repository.RootPath, ignoredDirectory)))
                        {
                            continue;
                        }
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
                    FindChangedFiles(
                        subRepository,
                        subRepositoryBuildStatus,
                        changedFiles,
                        forceBuild
                    );
                }
            }
        }

        private List<string> FindAffectedCsProjFiles(string repositoryPath, List<PatchEntryChanges> files)
        {
            var affectedProjectFiles = new List<string>();
            foreach (var file in files)
            {
                var filePath = Path.Combine(repositoryPath, file.Path);
                if (filePath.EndsWith(".csproj"))
                {
                    affectedProjectFiles.Add(filePath);
                }

                if (!filePath.EndsWith(".cs"))
                {
                    continue;
                }

                var classFile = new FileInfo(filePath);
                var csProjPath = FindBelongingProjectPathOfClass(classFile.Directory?.FullName);
                if (csProjPath.IsNullOrEmpty() || affectedProjectFiles.Contains(csProjPath))
                {
                    continue;
                }

                affectedProjectFiles.Add(csProjPath);
            }

            return affectedProjectFiles;
        }

        private string FindBelongingProjectPathOfClass(string directoryPath)
        {
            var files = Directory.GetFiles(directoryPath, "*.csproj", SearchOption.TopDirectoryOnly);
            if (files.Length == 1)
            {
                return files.First();
            }

            var directoryInfo = new DirectoryInfo(directoryPath);

            if (directoryInfo.Parent == null)
            {
                return null;
            }

            return FindBelongingProjectPathOfClass(directoryInfo.Parent.FullName);
        }
    }
}
