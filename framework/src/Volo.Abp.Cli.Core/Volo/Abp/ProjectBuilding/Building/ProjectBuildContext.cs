using JetBrains.Annotations;
using Volo.Abp.ProjectBuilding.Files;

namespace Volo.Abp.ProjectBuilding.Building
{
    public class ProjectBuildContext
    {
        [NotNull]
        public TemplateFile TemplateFile { get; }

        [NotNull]
        public string Version { get; }

        [NotNull]
        public ProjectBuildArgs BuildArgs { get; }

        [NotNull]
        public TemplateInfo Template { get; }

        public FileEntryList Files { get; set; }

        public ProjectResult Result { get; set; }
        
        public ProjectBuildContext([NotNull] TemplateInfo template,
            [NotNull] TemplateFile templateFile,
            [NotNull] ProjectBuildArgs buildArgs,
            [NotNull] string version)
        {
            Template = Check.NotNull(template, nameof(template));
            TemplateFile = Check.NotNull(templateFile, nameof(templateFile));
            BuildArgs = Check.NotNull(buildArgs, nameof(buildArgs));
            Version = Check.NotNullOrWhiteSpace(version, nameof(version));

            Result = new ProjectResult();
        }
    }
}