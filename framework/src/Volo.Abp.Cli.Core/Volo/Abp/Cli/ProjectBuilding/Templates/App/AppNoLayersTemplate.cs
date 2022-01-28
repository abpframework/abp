using System.Collections.Generic;
using Volo.Abp.Cli.ProjectBuilding.Building;

namespace Volo.Abp.Cli.ProjectBuilding.Templates.App;

public class AppNoLayersTemplate : AppTemplateBase
{
    /// <summary>
    /// "app-nolayers".
    /// </summary>
    public const string TemplateName = "app-nolayers";

    public AppNoLayersTemplate()
        : base(TemplateName)
    {
        //TODO: Change URL
        DocumentUrl = CliConsts.DocsLink + "/en/abp/latest/Startup-Templates/Application";
    }

    public override IEnumerable<ProjectBuildPipelineStep> GetCustomSteps(ProjectBuildContext context)
    {
        var steps = new List<ProjectBuildPipelineStep>();

        RandomizeSslPorts(context, steps);
        RandomizeStringEncryption(context, steps);

        return steps;
    }
}
