using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.ProjectBuilding.Building.Steps;
using Volo.Abp.Cli.ProjectBuilding.Files;

namespace Volo.Abp.Cli.ProjectBuilding.Templates.Mvc
{
    public class MvcTemplateProjectRenameStep : ProjectBuildPipelineStep
    {
        private readonly string _oldProjectName;
        private readonly string _newProjectName;

        public MvcTemplateProjectRenameStep(
            string oldProjectName,
            string newProjectName)
        {
            _oldProjectName = oldProjectName;
            _newProjectName = newProjectName;
        }

        public override void Execute(ProjectBuildContext context)
        {
            context
                .GetFile("/MyCompanyName.MyProjectName.sln")
                .ReplaceText(_oldProjectName, _newProjectName);
            
            RenameHelper.RenameAll(context.Files, _oldProjectName, _newProjectName);
        }
    }
}
