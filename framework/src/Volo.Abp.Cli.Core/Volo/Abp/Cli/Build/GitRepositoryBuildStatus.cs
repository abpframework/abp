using System;
using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.Cli.Build
{
    public class GitRepositoryBuildStatus
    {
        /// <summary>
        /// Name of the repository
        /// </summary>
        public string RepositoryName { get; set; }

        /// <summary>
        /// Branch of the repository
        /// </summary>
        public string BranchName { get; set; }

        /// <summary>
        /// Last succeeded commitId of the repository
        /// </summary>
        public string CommitId { get; set; }

        /// <summary>
        /// Build succeeded projects of this repository
        /// </summary>
        public List<DotNetProjectBuildStatus> SucceedProjects { get; set; }

        /// <summary>
        /// Build status of depending repositories
        /// </summary>
        public List<GitRepositoryBuildStatus> DependingRepositories { get; set; }

        public GitRepositoryBuildStatus(string repositoryName, string branchName)
        {
            RepositoryName = repositoryName;
            BranchName = branchName;
            SucceedProjects = new List<DotNetProjectBuildStatus>();
            DependingRepositories = new List<GitRepositoryBuildStatus>();
        }

        public GitRepositoryBuildStatus GetSelfOrChild(string repositoryName)
        {
            if (RepositoryName == repositoryName)
            {
                return this;
            }

            return GetChild(repositoryName);
        }

        public GitRepositoryBuildStatus GetChild(string repositoryName)
        {
            foreach (var dependingRepository in DependingRepositories)
            {
                var child = GetChildInternal(dependingRepository, repositoryName);
                if (child != null)
                {
                    return child;
                }
            }

            return null;
        }

        public string GetUniqueName(string prefix)
        {
            var name = RepositoryName + "_" + BranchName;
            foreach (var dependingRepository in DependingRepositories)
            {
                AddToUniqueName(dependingRepository, name);
            }

            return (prefix.IsNullOrEmpty() ? "" : prefix + "_") + name.ToMd5();
        }

        public void AddOrUpdateProjectStatus(DotNetProjectBuildStatus status)
        {
            var existingProjectStatus = SucceedProjects.FirstOrDefault(p => p.CsProjPath == status.CsProjPath);
            if (existingProjectStatus != null)
            {
                existingProjectStatus.CommitId = status.CommitId;
            }
            else
            {
                SucceedProjects.Add(status);
            }
        }

        public void MergeWith(GitRepositoryBuildStatus newBuildStatus)
        {
            if (!newBuildStatus.CommitId.IsNullOrEmpty())
            {
                CommitId = newBuildStatus.CommitId;
            }

            foreach (var succeedProject in newBuildStatus.SucceedProjects)
            {
                AddOrUpdateProjectStatus(succeedProject);
            }

            foreach (var dependingRepositoryBuildStatus in newBuildStatus.DependingRepositories)
            {
                var existingDependingRepositoryBuildStatus = GetChild(dependingRepositoryBuildStatus.RepositoryName);
                var newDependingRepositoryBuildStatus = newBuildStatus.GetChild(
                    dependingRepositoryBuildStatus.RepositoryName
                );

                existingDependingRepositoryBuildStatus.MergeWith(newDependingRepositoryBuildStatus);
            }
        }

        private GitRepositoryBuildStatus GetChildInternal(GitRepositoryBuildStatus repositoryBuildStatus,
            string repositoryName)
        {
            if (repositoryBuildStatus.RepositoryName == repositoryName)
            {
                return repositoryBuildStatus;
            }

            foreach (var dependingRepository in repositoryBuildStatus.DependingRepositories)
            {
                var child = GetChildInternal(dependingRepository, repositoryName);
                if (child != null)
                {
                    return child;
                }
            }

            return null;
        }

        private void AddToUniqueName(GitRepositoryBuildStatus gitRepository, string name)
        {
            name += "_" + gitRepository.RepositoryName + "_" + gitRepository.BranchName;

            foreach (var dependingRepository in gitRepository.DependingRepositories)
            {
                AddToUniqueName(dependingRepository, name);
            }
        }
    }
}
