using System.Linq;
using Volo.Abp.Cli.ProjectBuilding.Files;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps
{
    public class RemovePublicRedisStep : ProjectBuildPipelineStep
    {
        public override void Execute(ProjectBuildContext context)
        {
            context.Files.FirstOrDefault(f => f.Name.EndsWith("MyCompanyName.MyProjectName.Web.csproj"))?.RemoveTemplateCodeIfNot("PUBLIC-REDIS");
            context.Files.FirstOrDefault(f => f.Name.EndsWith("MyProjectNameWebModule.cs"))?.RemoveTemplateCodeIfNot("PUBLIC-REDIS");
            context.Files.FirstOrDefault(f => f.Name.EndsWith("MyCompanyName.MyProjectName.HttpApi.HostWithIds.csproj"))?.RemoveTemplateCodeIfNot("PUBLIC-REDIS");
            context.Files.FirstOrDefault(f => f.Name.EndsWith("MyCompanyName.MyProjectName.HttpApi.Host.csproj"))?.RemoveTemplateCodeIfNot("PUBLIC-REDIS");
            context.Files.FirstOrDefault(f => f.Name.EndsWith("MyProjectNameHttpApiHostModule.cs"))?.RemoveTemplateCodeIfNot("PUBLIC-REDIS");
        }
    }
}
