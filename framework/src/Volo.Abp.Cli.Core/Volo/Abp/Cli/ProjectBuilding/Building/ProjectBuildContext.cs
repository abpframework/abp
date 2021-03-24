using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.Cli.ProjectBuilding.Files;
using Volo.Abp.Cli.ProjectModification;

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

        public NugetPackageInfo Package { get; }

        public FileEntryList Files { get; set; }

        public ProjectResult Result { get; set; }
        
        public List<string> Symbols { get; } //TODO: Fill the symbols, like "UI-Angular", "CMS-KIT"!
        
        public ProjectBuildContext(
            TemplateInfo template,
            ModuleInfo module,
            NugetPackageInfo package,
            [NotNull] TemplateFile templateFile,
            [NotNull] ProjectBuildArgs buildArgs)
        {
            Template = template;
            Module = module;
            Package = package;
            TemplateFile = Check.NotNull(templateFile, nameof(templateFile));
            BuildArgs = Check.NotNull(buildArgs, nameof(buildArgs));
            Symbols = new List<string>();

            Result = new ProjectResult();
        }
    }
}
