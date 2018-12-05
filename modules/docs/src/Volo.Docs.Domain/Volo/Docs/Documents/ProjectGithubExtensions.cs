using System;
using Volo.Docs.Projects;

namespace Volo.Docs.Documents
{
    public static class ProjectGithubExtensions
    {
        public static string GetGithubUrl(this Projects.Project project)
        {
            CheckGithubProject(project);
            return project.ExtraProperties["GithubRootUrl"] as string;
        }

        public static void SetGithubUrl(this Projects.Project project, string value)
        {
            CheckGithubProject(project);
            project.ExtraProperties["GithubRootUrl"] = value;
        }

        private static void CheckGithubProject(Project project)
        {
            if (project.DocumentStoreType != GithubDocumentStore.Type)
            {
                throw new ApplicationException("Given project has not a Github document store!");
            }
        }
    }
}