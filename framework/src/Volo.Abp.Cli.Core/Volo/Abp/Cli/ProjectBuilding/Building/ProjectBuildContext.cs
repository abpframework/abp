using JetBrains.Annotations;
using Volo.Abp.Cli.ProjectBuilding.Files;

namespace Volo.Abp.Cli.ProjectBuilding.Building
{
    public class ProjectBuildContext
    {
        [NotNull]
        public TemplateFile TemplateFile { get; }

        [NotNull]
        public ProjectBuildArgs BuildArgs { get; }

        [NotNull]
        public TemplateInfo Template { get; }

        public FileEntryList Files { get; set; }

        public ProjectResult Result { get; set; }
        
        public ProjectBuildContext(
            [NotNull] TemplateInfo template,
            [NotNull] TemplateFile templateFile,
            [NotNull] ProjectBuildArgs buildArgs)
        {
            Template = Check.NotNull(template, nameof(template));
            TemplateFile = Check.NotNull(templateFile, nameof(templateFile));
            BuildArgs = Check.NotNull(buildArgs, nameof(buildArgs));

            Result = new ProjectResult();
        }
    }
}