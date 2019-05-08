using JetBrains.Annotations;

namespace Volo.Abp.ProjectModification
{
    public class AddModuleArgs
    {
        [NotNull]
        public string ModuleName { get; set; }

        [CanBeNull]
        public string SolutionFile { get; set; }

        [CanBeNull]
        public string ProjectFile { get; set; }

        public AddModuleArgs(
            [NotNull] string moduleName,
            [CanBeNull] string solutionFile = null,
            [CanBeNull] string projectFile = null)
        {
            ModuleName = Check.NotNullOrWhiteSpace(moduleName, nameof(moduleName));
            SolutionFile = solutionFile;
            ProjectFile = projectFile;
        }
    }
}
