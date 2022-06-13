using System;
using System.Linq;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps;

public class ChangeApplicationIdGuidStep: ProjectBuildPipelineStep
{
    public override void Execute(ProjectBuildContext context)
    {
        var projectFile = context.Files.FirstOrDefault(f => f.Name.EndsWith("MyCompanyName.MyProjectName.csproj"));
        
        projectFile?.SetContent(projectFile.Content.Replace("27317750-B571-4690-B433-B358B2480E01", Guid.NewGuid().ToString()));
    }
}