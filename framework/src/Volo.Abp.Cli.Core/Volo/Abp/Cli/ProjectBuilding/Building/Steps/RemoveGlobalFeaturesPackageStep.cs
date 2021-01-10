using System.Linq;
using Volo.Abp.Cli.ProjectBuilding.Files;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps
{
    public class RemoveGlobalFeaturesPackageStep : ProjectBuildPipelineStep
    {
        public override void Execute(ProjectBuildContext context)
        {
            context.Files.FirstOrDefault(f => f.Name.EndsWith("MyCompanyName.MyProjectName.Domain.Shared.csproj"))?.RemoveTemplateCodeIf("CMS-KIT");
            context.Files.FirstOrDefault(f => f.Name.EndsWith("MyProjectNameDomainSharedModule.cs"))?.RemoveTemplateCodeIf("CMS-KIT");
        }
    }
}
