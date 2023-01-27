using System;
using System.Linq;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps;

public class AppNoLayersMoveProjectsStep : ProjectBuildPipelineStep
{
    public override void Execute(ProjectBuildContext context)
    {
        MoveFiles(context, "/aspnet-core/MyCompanyName.MyProjectName.Blazor.WebAssembly/Client",
            "/aspnet-core/MyCompanyName.MyProjectName.Blazor");
        MoveFiles(context, "/aspnet-core/MyCompanyName.MyProjectName.Blazor.WebAssembly/Server", 
            "/aspnet-core/MyCompanyName.MyProjectName.Host");
        MoveFiles(context, "/aspnet-core/MyCompanyName.MyProjectName.Blazor.WebAssembly/Shared", 
            "/aspnet-core/MyCompanyName.MyProjectName.Contracts");
        
        ModifySolutionFile(context, "MyCompanyName.MyProjectName.Blazor.WebAssembly\\Client\\MyCompanyName.MyProjectName.Blazor.csproj",
            "MyCompanyName.MyProjectName.Blazor\\MyCompanyName.MyProjectName.Blazor.csproj");
        ModifySolutionFile(context, "MyCompanyName.MyProjectName.Blazor.WebAssembly\\Server\\MyCompanyName.MyProjectName.Host.csproj",
            "MyCompanyName.MyProjectName.Host\\MyCompanyName.MyProjectName.Host.csproj");
        ModifySolutionFile(context, "MyCompanyName.MyProjectName.Blazor.WebAssembly\\Server.Mongo\\MyCompanyName.MyProjectName.Host.csproj",
            "MyCompanyName.MyProjectName.Host\\MyCompanyName.MyProjectName.Host.csproj");
        ModifySolutionFile(context, "MyCompanyName.MyProjectName.Blazor.WebAssembly\\Shared\\MyCompanyName.MyProjectName.Contracts.csproj",
            "MyCompanyName.MyProjectName.Contracts\\MyCompanyName.MyProjectName.Contracts.csproj");
        
        ModifyProjectFiles(context, "..\\Shared\\MyCompanyName.MyProjectName.Blazor.WebAssembly.Shared.csproj",
            "..\\MyCompanyName.MyProjectName.Contracts\\MyCompanyName.MyProjectName.Contracts.csproj");
        
        ModifyProjectFiles(context, "..\\Client\\MyCompanyName.MyProjectName.Blazor.WebAssembly.Client.csproj",
            "..\\MyCompanyName.MyProjectName.Blazor\\MyCompanyName.MyProjectName.Blazor.csproj");
    }

    public void MoveFiles(ProjectBuildContext context, string projectFolder, string newProjectFolder)
    {
        var fileEntries = context.Files.Where(file => file.Name.StartsWith(projectFolder));
        foreach (var fileEntry in fileEntries)
        {
            fileEntry.SetName(fileEntry.Name.ReplaceFirst(projectFolder, newProjectFolder));
        }
    }

    public void ModifySolutionFile(ProjectBuildContext context, string pathInSlnFile, string newPathInSlnFile)
    {
        var slnFile = context.Files.First(file => file.Name.EndsWith(".sln"));
        slnFile.SetContent(slnFile.Content.Replace($"\"{pathInSlnFile}\"", $"\"{newPathInSlnFile}\""));
    }

    public void ModifyProjectFiles(ProjectBuildContext context, string oldContent, string newContent)
    {
        var projectFiles = context.Files.Where(file => file.Name.EndsWith(".csproj"));

        foreach (var projectFile in projectFiles)
        {
            projectFile.SetContent(projectFile.Content.Replace(oldContent, newContent));
        }
    }
}
