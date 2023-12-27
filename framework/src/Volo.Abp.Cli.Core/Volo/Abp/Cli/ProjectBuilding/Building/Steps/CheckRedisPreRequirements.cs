using System;
using System.Linq;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps;

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
