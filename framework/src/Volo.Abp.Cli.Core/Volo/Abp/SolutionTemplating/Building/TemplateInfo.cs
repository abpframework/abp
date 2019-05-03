using System;
using System.Collections.Generic;

namespace Volo.Abp.SolutionTemplating.Building
{
    public abstract class TemplateInfo
    {
        public string Name { get; set; }

        public GithubRepositoryInfo GithubRepository { get; }

        public string FilePath { get; set; }

        public string RootPathInZipFile { get; set; }

        public string Version { get; set; }

        protected TemplateInfo(
            string name,
            GithubRepositoryInfo githubRepository,
            string rootPathInZipFile)
        {
            Name = name;
            GithubRepository = githubRepository;
            RootPathInZipFile = rootPathInZipFile;
        }

        public virtual IEnumerable<ProjectBuildPipelineStep> GetCustomSteps(ProjectBuildContext context)
        {
            return Array.Empty<ProjectBuildPipelineStep>();
        }
    }
}