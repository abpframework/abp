namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps;

public class ChangeLocalhostPortStep : ProjectBuildPipelineStep
{
    public string LaunchSettingsFilePath { get; }
    public int[] PortNumbers { get; }

    public ChangeLocalhostPortStep(
        string launchSettingsFilePath,
        params int[] portNumbers)
    {
        Check.NotNullOrEmpty(portNumbers, nameof(portNumbers));

        LaunchSettingsFilePath = launchSettingsFilePath;
        PortNumbers = portNumbers;
    }

    public override void Execute(ProjectBuildContext context)
    {
        var launchSettings = context.GetFile(LaunchSettingsFilePath);
        var randomPort = RandomHelper.GetRandom(50001, 59990);
        var newContent = launchSettings.Content;

        for (var i = 0; i < PortNumbers.Length; i++)
        {
            newContent = newContent.Replace(
                PortNumbers[i].ToString(),
                (randomPort + i).ToString()
            );
        }

        launchSettings.SetContent(newContent);
    }
}
