using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.Cli.ProjectBuilding.Building;

namespace Volo.Abp.Cli.ProjectBuilding
{
    public class ProjectBuildArgs
    {
        [NotNull]
        public SolutionName SolutionName { get; }

        [CanBeNull]
        public string TemplateName { get; set; }

        public DatabaseProvider DatabaseProvider { get; set; }

        [NotNull]
        public Dictionary<string, string> ExtraProperties { get; set; }

        public ProjectBuildArgs(
            [NotNull] SolutionName solutionName, 
            [CanBeNull] string templateName = null,
            DatabaseProvider databaseProvider = DatabaseProvider.NotSpecified,
            Dictionary<string, string> extraProperties = null)
        {
            DatabaseProvider = databaseProvider;
            TemplateName = templateName;
            SolutionName = Check.NotNull(solutionName, nameof(solutionName));

            ExtraProperties = extraProperties ?? new Dictionary<string, string>();
        }
    }
}