using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Docs.GitHub.Documents;
using Volo.Docs.Projects;

namespace Volo.Docs.GitHub.Projects
{
    public static class ProjectGithubExtensions
    {
        public static string GetGitHubUrl([NotNull] this Project project)
        {
            CheckGitHubProject(project);
            return project.ExtraProperties["GitHubRootUrl"] as string;
        }

        public static string GetGitHubInnerUrl([NotNull] this Project project, string languageCode, string documentName)
        {
            return project
                       .GetGitHubUrl().Split("{version}")[1].EnsureEndsWith('/').TrimStart('/') + languageCode + '/' + documentName;
        }

        public static string GetGitHubUrl([NotNull] this Project project, string version)
        {
            return project
                .GetGitHubUrl()
                .Replace("{version}", version);
        }

        public static void SetGitHubUrl([NotNull] this Project project, string value)
        {
            CheckGitHubProject(project);
            project.ExtraProperties["GitHubRootUrl"] = value;
        }

        public static string GetGitHubAccessTokenOrNull([NotNull] this Project project)
        {
            CheckGitHubProject(project);
            return project.ExtraProperties["GitHubAccessToken"] as string;
        }

        public static string GetGithubUserAgentOrNull([NotNull] this Project project)
        {
            CheckGitHubProject(project);
            return project.ExtraProperties["GitHubUserAgent"] as string;
        }

        public static void SetGitHubAccessToken([NotNull] this Project project, string value)
        {
            CheckGitHubProject(project);
            project.ExtraProperties["GitHubAccessToken"] = value;
        }

        private static void CheckGitHubProject(Project project)
        {
            Check.NotNull(project, nameof(project));

            if (project.DocumentStoreType != GithubDocumentSource.Type)
            {
                throw new ApplicationException("Given project has not a Github document store!");
            }
        }
    }
}