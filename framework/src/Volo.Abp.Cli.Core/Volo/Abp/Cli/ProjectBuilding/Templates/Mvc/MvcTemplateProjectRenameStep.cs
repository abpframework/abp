using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.ProjectBuilding.Building.Steps;

namespace Volo.Abp.Cli.ProjectBuilding.Templates.Mvc
{
    public class MvcTemplateProjectRenameStep : ProjectBuildPipelineStep
    {
        private readonly string _oldProjectName;
        private readonly string _newProjectName;
        private readonly string _folder;

        public MvcTemplateProjectRenameStep(
            string oldProjectName, 
            string newProjectName,
            string folder = "/src/")
        {
            _oldProjectName = oldProjectName;
            _newProjectName = newProjectName;
            _folder = folder;
        }

        public override void Execute(ProjectBuildContext context)
        {
            ReplaceInFile(
                context,
                "/MyCompanyName.MyProjectName.sln",
                _oldProjectName,
                _newProjectName
            );

            RenameHelper.RenameAll(context.Files, _oldProjectName, _newProjectName);
        }

        public void ReplaceInFile(
            ProjectBuildContext context,
            string filePath,
            string oldText,
            string newText)
        {
            var file = context.GetFile(filePath);
            file.NormalizeLineEndings();
            file.SetContent(file.Content.Replace(oldText, newText));
        }
    }
}
