using System;
using System.Collections.Generic;

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
                return GetChildInternal(dependingRepository, repositoryName);
            }

            return null;
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
                return GetChildInternal(dependingRepository, repositoryName);
            }

            return null;
        }

        public string GetUniqueName(string uniqueName)
        {
            var name = RepositoryName + "_" + BranchName;
            foreach (var dependingRepository in DependingRepositories)
            {
                AddToUniqueName(dependingRepository, name);
            }

            return (uniqueName.IsNullOrEmpty() ? "" : uniqueName + "_") + name.ToMd5();
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
