using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LibGit2Sharp;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Build
{
    public class DefaultChangedProjectFinder : IChangedProjectFinder, ITransientDependency
    {
        private readonly IRepositoryBuildStatusStore _repositoryBuildStatusStore;
        private readonly IGitRepositoryHelper _gitRepositoryHelper;

        private readonly IDotNetProjectDependencyFiller _dotNetProjectDependencyFiller;
        private readonly IBuildProjectListSorter _buildProjectListSorter;

        private readonly List<string> _changeDetectionFileExtensions = new List<string>
        {
            ".cs",
            ".csproj",
            ".cshtml"
        };

        public DefaultChangedProjectFinder(
            IRepositoryBuildStatusStore repositoryBuildStatusStore,
            IGitRepositoryHelper gitRepositoryHelper,
            IDotNetProjectDependencyFiller dotNetProjectDependencyFiller,
            IBuildProjectListSorter buildProjectListSorter)
        {
            _repositoryBuildStatusStore = repositoryBuildStatusStore;
            _gitRepositoryHelper = gitRepositoryHelper;
            _dotNetProjectDependencyFiller = dotNetProjectDependencyFiller;
            _buildProjectListSorter = buildProjectListSorter;
        }

        /// <summary>
        /// Returns list of projects in a repository and its depending repositories sorted by dependencies
        /// </summary>
        /// <param name="buildConfig"></param>
        /// <returns></returns>
        private List<DotNetProjectInfo> FindAllProjects(DotNetProjectBuildConfig buildConfig)
        {
            var projects = new List<DotNetProjectInfo>();

            AddProjectsOfRepository(buildConfig.GitRepository, projects);

            _dotNetProjectDependencyFiller.Fill(projects);

            var allSortedProjectList = _buildProjectListSorter.SortByDependencies(
                projects,
                new DotNetProjectInfoEqualityComparer()
            );

            return FilterIgnoredDirectories(allSortedProjectList, buildConfig.GitRepository);
        }

        private List<DotNetProjectInfo> FilterIgnoredDirectories(List<DotNetProjectInfo> projects,
            GitRepository gitRepository)
        {
            foreach (var ignoredDirectory in gitRepository.IgnoredDirectories)
            {
                Console.WriteLine("projects count 1:" + projects.Count);
                Console.WriteLine("Ignoring Directory...:" + Path.Combine(gitRepository.RootPath, ignoredDirectory));

                projects = projects.Where(e =>
                        !e.CsProjPath.StartsWith(Path.Combine(gitRepository.RootPath, ignoredDirectory)))
                    .ToList();

                Console.WriteLine("projects count 2:" + projects.Count);
            }

            foreach (var dependingRepository in gitRepository.DependingRepositories)
            {
                FilterIgnoredDirectories(projects, dependingRepository);
            }

            return projects;
        }

        private void AddProjectsOfRepository(GitRepository gitRepository, List<DotNetProjectInfo> projects)
        {
            var allCsProjFiles = Directory.GetFiles(
                gitRepository.RootPath,
                "*.csproj",
                SearchOption.AllDirectories
            ).ToList();

            projects.AddRange(
                allCsProjFiles.Select(csProjPath => new DotNetProjectInfo(gitRepository.Name, csProjPath, false))
            );

            foreach (var dependingRepository in gitRepository.DependingRepositories)
            {
                AddProjectsOfRepository(dependingRepository, projects);
            }
        }

        public List<DotNetProjectInfo> FindByRepository(DotNetProjectBuildConfig buildConfig)
        {
            Console.WriteLine("Finding changed projects for repository...");
            
            var gitRepositoryBuildStatus = _repositoryBuildStatusStore.Get(
                buildConfig.BuildName,
                buildConfig.GitRepository
            );

            var allSortedProjectList = FindAllProjects(buildConfig);

            MarkProjectsForBuild(
                buildConfig.GitRepository,
                gitRepositoryBuildStatus,
                buildConfig.ForceBuild,
                allSortedProjectList
            );

            return allSortedProjectList.Where(e => e.ShouldBuild).ToList();
        }

        private void MarkProjectsForBuild(
            GitRepository repository,
            GitRepositoryBuildStatus repositoryBuildStatus,
            bool forceBuild,
            List<DotNetProjectInfo> allProjectList)
        {
            if (forceBuild || repositoryBuildStatus == null || repositoryBuildStatus.CommitId.IsNullOrEmpty())
            {
                // Mark all projects for build
                allProjectList.ForEach(e => e.ShouldBuild = true);
            }
            else
            {
                MarkChangedProjectsForBuild(
                    repository,
                    repositoryBuildStatus,
                    allProjectList
                );
            }

            if (!repository.DependingRepositories.Any())
            {
                return;
            }

            foreach (var dependingRepository in repository.DependingRepositories)
            {
                var dependingRepositoryBuildStatus = repositoryBuildStatus?.GetChild(dependingRepository.Name);
                MarkProjectsForBuild(
                    dependingRepository,
                    dependingRepositoryBuildStatus,
                    forceBuild,
                    allProjectList
                );
            }
        }

        private void MarkChangedProjectsForBuild(
            GitRepository repository,
            GitRepositoryBuildStatus status,
            List<DotNetProjectInfo> allProjectList)
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
                    fileExtensionPredicate = fileExtensionPredicate.Or(
                        e => e.Path.EndsWith(changeDetectionFileExtension)
                    );
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
                    var isIgnored = repository.IgnoredDirectories.Any(ignoredDirectory =>
                        csProjPath.StartsWith(Path.Combine(repository.RootPath, ignoredDirectory)));
                    if (isIgnored)
                    {
                        continue;
                    }

                    allProjectList.MarkForBuild(repository.Name, csProjPath);
                    AddDependingProjectsToList(repository.Name, csProjPath, allProjectList);
                }
            }
        }
        
        private void AddDependingProjectsToList(
            string repositoryName,
            string csProjPath,
            List<DotNetProjectInfo> allProjectList)
        {
            var dependingProjects = allProjectList.Where(
                e => e.Dependencies.Any(d => d.RepositoryName == repositoryName && d.CsProjPath == csProjPath)
            ).Select(e => new DotNetProjectInfo(e.RepositoryName, e.CsProjPath, true)).ToList();

            if (!dependingProjects.Any())
            {
                return;
            }

            foreach (var dependingProject in dependingProjects)
            {
                if (allProjectList.IsMarkedForBuild(dependingProject))
                {
                    continue;
                }

                allProjectList.MarkForBuild(dependingProject);
                AddDependingProjectsToList(dependingProject.RepositoryName, dependingProject.CsProjPath,
                    allProjectList);
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
