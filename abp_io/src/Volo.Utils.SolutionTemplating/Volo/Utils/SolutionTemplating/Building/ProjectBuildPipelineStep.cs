using System;
using System.Linq;
using Volo.Utils.SolutionTemplating.Files;

namespace Volo.Utils.SolutionTemplating.Building
{
    public abstract class ProjectBuildPipelineStep
    {
        public abstract void Execute(ProjectBuildContext context);

        protected FileEntry GetFile(ProjectBuildContext context, string filePath)
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