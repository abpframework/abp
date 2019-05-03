using JetBrains.Annotations;
using Volo.Abp.ProjectBuilding.Building;

namespace Volo.Abp.ProjectBuilding
{
    public class ProjectBuildArgs
    {
        [NotNull]
        public SolutionName SolutionName { get; }

        [CanBeNull]
        public string TemplateName { get; set; }

        public DatabaseProvider DatabaseProvider { get; set; }

        public ProjectBuildArgs(
            [NotNull] SolutionName solutionName, 
            DatabaseProvider databaseProvider = DatabaseProvider.NotSpecified, 
            [CanBeNull] string templateName = null)
        {
            DatabaseProvider = databaseProvider;
            TemplateName = templateName;
            SolutionName = Check.NotNull(solutionName, nameof(solutionName));
        }
    }
}