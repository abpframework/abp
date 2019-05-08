using System;
using System.Linq;
using Volo.Abp.ProjectBuilding.Files;

namespace Volo.Abp.ProjectBuilding.Building
{
    public static class ProjectBuildContextExtensions
    {
        public static FileEntry GetFile(this ProjectBuildContext context, string filePath)
        {
            var file = context.Files.FirstOrDefault(f => f.Name == filePath);
            if (file == null)
            {
                throw new ApplicationException("Could not find file: " + filePath);
            }

            return file;
        }
    }
}