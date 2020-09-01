using System.Collections.Generic;
using System.Linq;
using LibGit2Sharp;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Build
{
    public class DefaultBuildStatusGenerator : IBuildStatusGenerator, ITransientDependency
    {
        public GitRepositoryBuildStatus Generate(GitRepository gitRepository, List<DotNetProjectInfo> changedProjects,
            List<string> buildSucceededProjects)
        {
            using (var repo = new Repository(string.Concat(gitRepository.RootPath, @"\.git")))
            {
                var lastCommitId = repo.Head.Tip.Id.ToString();
                var status = new GitRepositoryBuildStatus(
                    gitRepository.Name,
                    repo.Head.FriendlyName
                )
                {
                    CommitId = lastCommitId
                };

                status.SucceedProjects = changedProjects.Where(p =>
                        p.RepositoryName == gitRepository.Name &&
                        buildSucceededProjects.Contains(p.CsProjPath)
                    )
                    .Select(e => new DotNetProjectBuildStatus()
                    {
                        CsProjPath = e.CsProjPath,
                        CommitId = lastCommitId
                    }).ToList();

                foreach (var dependingRepository in gitRepository.DependingRepositories)
                {
                    GenerateBuildStatusInternal(dependingRepository, changedProjects, buildSucceededProjects, status);
                }

                return status;
            }
        }

        private void GenerateBuildStatusInternal(
            GitRepository gitRepository,
            List<DotNetProjectInfo> changedProjects,
            List<string> buildSucceededProjects,
            GitRepositoryBuildStatus status)
        {
            using (var repo = new Repository(string.Concat(gitRepository.RootPath, @"\.git")))
            {
                var lastCommitId = repo.Head.Tip.Id.ToString();
                var dependingRepositoryStatus = new GitRepositoryBuildStatus(
                    gitRepository.Name,
                    repo.Head.FriendlyName
                )
                {
                    CommitId = lastCommitId
                };

                dependingRepositoryStatus.SucceedProjects = changedProjects.Where(p =>
                        p.RepositoryName == gitRepository.Name &&
                        buildSucceededProjects.Contains(p.CsProjPath)
                    )
                    .Select(e => new DotNetProjectBuildStatus()
                    {
                        CsProjPath = e.CsProjPath,
                        CommitId = lastCommitId
                    }).ToList();

                foreach (var dependingRepository in gitRepository.DependingRepositories)
                {
                    GenerateBuildStatusInternal(
                        dependingRepository,
                        changedProjects,
                        buildSucceededProjects,
                        dependingRepositoryStatus
                    );
                }

                status.DependingRepositories.Add(dependingRepositoryStatus);
            }
        }
    }
}