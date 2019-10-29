using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.Cli.ProjectBuilding.Building
{
    public abstract class TemplateInfo
    {
        [NotNull]
        public string Name { get; }

        public DatabaseProvider DefaultDatabaseProvider { get; }

        public UiFramework DefaultUiFramework { get; }

        [CanBeNull]
        public string DocumentUrl { get; set; }

        protected TemplateInfo(
            [NotNull] string name, 
            DatabaseProvider defaultDatabaseProvider = DatabaseProvider.NotSpecified,
            UiFramework defaultUiFramework = UiFramework.NotSpecified)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            DefaultDatabaseProvider = defaultDatabaseProvider;
            DefaultUiFramework = defaultUiFramework;
        }

        public virtual IEnumerable<ProjectBuildPipelineStep> GetCustomSteps(ProjectBuildContext context)
        {
            return Array.Empty<ProjectBuildPipelineStep>();
        }
    }
}