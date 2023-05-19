using System;
using System.IO;
using System.Linq;
using Volo.Abp.Cli.Commands;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps;

public class MicroserviceServiceRandomPortStep : ProjectBuildPipelineStep
{
    private readonly string _defaultPort = string.Empty;
    private string _tyeFileContent = null;

    public MicroserviceServiceRandomPortStep(string defaultPort)
    {
        _defaultPort = defaultPort;
    }

    public override void Execute(ProjectBuildContext context)
    {
        var newPort = GetNewRandomPort(context);

        var targetFiles = context.Files.Where(f => f.Name.EndsWith("launchSettings.json") || f.Name.EndsWith("appsettings.json")).ToList();

        foreach (var file in targetFiles)
        {
            file.SetContent(file.Content.Replace(_defaultPort, newPort));
        }
    }

    private string GetNewRandomPort(ProjectBuildContext context)
    {
        string newPort;
        var rnd = new Random();
        var tryCount = 0;

        do
        {
            newPort = rnd.Next(44350, 45350).ToString();

            if (tryCount++ > 2000)
            {
                break;
            }

        } while (PortExistsForAnotherService(context, newPort));

        return newPort;
    }

    private bool PortExistsForAnotherService(ProjectBuildContext context, string newPort)
    {
        return ReadTyeFileContent(context).SplitToLines().Any(l => l.Contains("port") && l.Contains(newPort));
    }

    private string ReadTyeFileContent(ProjectBuildContext context)
    {
        if (_tyeFileContent != null)
        {
            return _tyeFileContent;
        }

        var solutionFolderPath = GetSolutionFolderPath(context);

        var tyeFilePath = Path.Combine(solutionFolderPath, "tye.yaml");

        if (!File.Exists(tyeFilePath))
        {
            return String.Empty;
        }

        _tyeFileContent = File.ReadAllText(tyeFilePath);

        return _tyeFileContent;
    }

    private static string GetSolutionFolderPath(ProjectBuildContext context)
    {
        if (context.BuildArgs.ExtraProperties.ContainsKey(NewCommand.Options.OutputFolder.Short))
        {
            return context.BuildArgs.ExtraProperties[NewCommand.Options.OutputFolder.Short];
        }

        if (context.BuildArgs.ExtraProperties.ContainsKey(NewCommand.Options.OutputFolder.Long))
        {
            return context.BuildArgs.ExtraProperties[NewCommand.Options.OutputFolder.Long];
        }

        return Directory.GetCurrentDirectory();
    }
}
