using Volo.Abp.Cli.ProjectBuilding.Building;

namespace Volo.Abp.Cli.Args;

public class AngularThemeConfigurationArgs 
{
    public Theme Theme { get; }

    public string ProjectName { get; }

    public string AngularFolderPath { get; }

    public AngularThemeConfigurationArgs(Theme theme, string projectName, string angularFolderPath)
    {
        Theme = theme;
        ProjectName = projectName;
        AngularFolderPath = angularFolderPath;
    }
}