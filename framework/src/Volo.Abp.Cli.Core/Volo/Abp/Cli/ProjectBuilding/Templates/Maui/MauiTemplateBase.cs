using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.Cli.ProjectBuilding.Building;

namespace Volo.Abp.Cli.ProjectBuilding.Templates.Maui;

public class MauiTemplateBase: TemplateInfo
{
    protected MauiTemplateBase([NotNull] string name) :
        base(name)
    {
    }

    public override IEnumerable<ProjectBuildPipelineStep> GetCustomSteps(ProjectBuildContext context)
    {
        var steps = new List<ProjectBuildPipelineStep>
        {
            new MauiChangeApplicationIdGuidStep()
        };
        
        return steps;
    }
}