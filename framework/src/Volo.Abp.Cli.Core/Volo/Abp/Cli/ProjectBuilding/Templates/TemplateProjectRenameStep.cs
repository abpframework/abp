using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.ProjectBuilding.Building.Steps;

namespace Volo.Abp.Cli.ProjectBuilding.Templates;

public class TemplateProjectRenameStep : ProjectBuildPipelineStep
{
    private readonly string _oldProjectName;
    private readonly string _newProjectName;

    public TemplateProjectRenameStep(
        string oldProjectName,
        string newProjectName)
    {
        _oldProjectName = oldProjectName;
        _newProjectName = newProjectName;
    }

    public override void Execute(ProjectBuildContext context)
    {
        RenameHelper.RenameAll(context.Files, _oldProjectName, _newProjectName);
    }
}
