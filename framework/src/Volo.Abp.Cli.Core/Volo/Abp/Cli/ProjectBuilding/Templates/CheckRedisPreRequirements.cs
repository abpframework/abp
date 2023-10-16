using System;
using System.Linq;
using Volo.Abp.Cli.ProjectBuilding.Building;

namespace Volo.Abp.Cli.ProjectBuilding.Templates;

public class CheckRedisPreRequirements : ProjectBuildPipelineStep
{
    public override void Execute(ProjectBuildContext context)
    {
        var modules = context.Files.Where(f => f.Name.EndsWith("Module.cs", StringComparison.OrdinalIgnoreCase));
        if (modules.Any(module => module.Content.Contains("Redis:Configuration")))
        {
            context.BuildArgs.ExtraProperties["PreRequirements:Redis"] = "true";
        }
    }
}
