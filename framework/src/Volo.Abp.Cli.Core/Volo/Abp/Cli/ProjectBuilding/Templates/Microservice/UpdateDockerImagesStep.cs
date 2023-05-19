using System.Linq;
using System.Runtime.InteropServices;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.ProjectBuilding.Files;

namespace Volo.Abp.Cli.ProjectBuilding.Templates.Microservice;

public class UpdateDockerImagesStep : ProjectBuildPipelineStep
{
    private readonly string _ymlFilePath;

    public UpdateDockerImagesStep(string ymlFilePath)
    {
        _ymlFilePath = ymlFilePath;
    }

    public override void Execute(ProjectBuildContext context)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) && RuntimeInformation.OSArchitecture == Architecture.Arm64)
        {
            var file = context.Files.FirstOrDefault(f => f.Name == _ymlFilePath);
            file?.ReplaceText("mcr.microsoft.com/mssql/server:2019-latest", "mcr.microsoft.com/azure-sql-edge");
        }
    }
}
