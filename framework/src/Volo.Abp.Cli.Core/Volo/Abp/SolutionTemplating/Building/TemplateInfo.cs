using System;
using System.Collections.Generic;

namespace Volo.Abp.SolutionTemplating.Building
{
    public abstract class TemplateInfo
    {
        public string FilePath { get; set; }

        public virtual IEnumerable<ProjectBuildPipelineStep> GetCustomSteps(ProjectBuildContext context)
        {
            return Array.Empty<ProjectBuildPipelineStep>();
        }
    }
}