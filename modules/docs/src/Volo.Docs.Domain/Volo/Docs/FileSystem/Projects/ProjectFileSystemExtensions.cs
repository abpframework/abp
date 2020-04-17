using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Docs.FileSystem.Documents;
using Volo.Docs.Projects;

namespace Volo.Docs.FileSystem.Projects
{
    public static class ProjectFileSystemExtensions
    {
        public static string GetFileSystemPath([NotNull] this Project project)
        {
            CheckFileSystemProject(project);
            return project.ExtraProperties["Path"] as string;
        }

        public static void SetFileSystemPath([NotNull] this Project project, string value)
        {
            CheckFileSystemProject(project);
            project.ExtraProperties["Path"] = value;
        }

        private static void CheckFileSystemProject(Project project)
        {
            Check.NotNull(project, nameof(project));

            if (project.DocumentStoreType != FileSystemDocumentSource.Type)
            {
                throw new ApplicationException("Given project has not a FileSystem document store!");
            }
        }
    }
}