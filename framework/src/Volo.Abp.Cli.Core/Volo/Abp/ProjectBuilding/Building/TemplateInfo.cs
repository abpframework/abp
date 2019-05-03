using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.ProjectBuilding.Building
{
    public abstract class TemplateInfo
    {
        public string Name { get; }

        public DatabaseProvider DefaultDatabaseProvider { get; }

        protected TemplateInfo(
            [NotNull] string name, 
            DatabaseProvider defaultDatabaseProvider = DatabaseProvider.NotSpecified)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            DefaultDatabaseProvider = Check.NotNull(defaultDatabaseProvider, nameof(defaultDatabaseProvider));
        }

        public virtual IEnumerable<ProjectBuildPipelineStep> GetCustomSteps(ProjectBuildContext context)
        {
            return Array.Empty<ProjectBuildPipelineStep>();
        }
    }
}