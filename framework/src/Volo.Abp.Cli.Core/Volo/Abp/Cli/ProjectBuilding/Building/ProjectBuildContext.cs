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

        public TemplateInfo Template { get; }

        public ModuleInfo Module { get; }

        public FileEntryList Files { get; set; }

        public ProjectResult Result { get; set; }
        
        public ProjectBuildContext(
            TemplateInfo template,
            ModuleInfo module,
            [NotNull] TemplateFile templateFile,
            [NotNull] ProjectBuildArgs buildArgs)
        {
            Template = template;
            Module = module;
            TemplateFile = Check.NotNull(templateFile, nameof(templateFile));
            BuildArgs = Check.NotNull(buildArgs, nameof(buildArgs));

            Result = new ProjectResult();
        }
    }
}