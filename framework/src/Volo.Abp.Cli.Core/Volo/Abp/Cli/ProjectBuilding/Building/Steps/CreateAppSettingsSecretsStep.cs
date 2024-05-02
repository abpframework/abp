using System;
using System.Linq;
using Volo.Abp.Cli.ProjectBuilding.Files;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps;

public class CreateAppSettingsSecretsStep : ProjectBuildPipelineStep
{
    private const string AppSettingsPlaceholder = "<!--APPSETTINGS-SECRETS-->";

    public override void Execute(ProjectBuildContext context)
    {
        var appSettingsFiles = context.Files
            .Where(x => x.Name.EndsWith(CliConsts.AppSettingsJsonFileName) && NotBlazorWasmProject(x.Name))
            .ToList();

        if (!appSettingsFiles.Any())
        {
            return;
        }

        var appsettingsSecretJsonContent = GetAppSettingsSecretJsonContent(context);
       
        foreach (var appSettingsFile in appSettingsFiles)
        {
            context.Files.Add(new FileEntry(
                appSettingsFile.Name.Replace(CliConsts.AppSettingsJsonFileName, CliConsts.AppSettingsSecretJsonFileName),
                appsettingsSecretJsonContent,
                false)
            );
        }

        var projectFiles = context.Files.Where(x => 
            x.Name.EndsWith(".csproj") &&
            x.Content.Contains(AppSettingsPlaceholder)).ToList();

        foreach (var projectFile in projectFiles)
        {
            projectFile.SetContent(ReplaceAppSettingsSecretsPlaceholder(projectFile.Content));
        }
    }

    private static byte[] GetAppSettingsSecretJsonContent(ProjectBuildContext context)
    {
        bool condition;
        if (context.Template != null)
        {
            condition = context.Template.IsPro();
        }
        else if (context.Module != null)
        {
            condition = context.Module.IsPro;
        }
        else
        {
            condition = false;
        }
        
        return condition
            ? $"{{{Environment.NewLine}  \"AbpLicenseCode\": \"{CliConsts.LicenseCodePlaceHolder}\" {Environment.NewLine}}}".GetBytes()
            : $"{{{Environment.NewLine}}}".GetBytes();
    }

    private static bool NotBlazorWasmProject(string fileName)
    {
        return !fileName.Contains("Blazor/wwwroot") && !fileName.Contains("Blazor.Host/wwwroot");
    }

    private static string ReplaceAppSettingsSecretsPlaceholder(string content)
    {
        var path = string.Empty;
        
        var appSettingsRemoveLine = content.SplitToLines().FirstOrDefault(l=>
            l.Contains("None") &&
            l.Contains("Remove") &&
            l.Contains(CliConsts.AppSettingsJsonFileName));

        if (appSettingsRemoveLine != null)
        {
             var prefix = appSettingsRemoveLine.Split("\"")
                .Select(s => s.Trim())
                .First(s => s.Contains(CliConsts.AppSettingsJsonFileName))
                .Replace(CliConsts.AppSettingsJsonFileName, "");

             path = string.IsNullOrWhiteSpace(prefix) ? string.Empty : prefix;
        }
        
        
        var replaceContent = $"<None Remove=\"{ path + CliConsts.AppSettingsSecretJsonFileName}\" />{Environment.NewLine}" +
                $"    <Content Include=\"{path + CliConsts.AppSettingsSecretJsonFileName}\">{Environment.NewLine}" +
                $"      <CopyToPublishDirectory>PreserveNewest</CopyToPublishDirectory>{Environment.NewLine}" +
                $"      <CopyToOutputDirectory>Always</CopyToOutputDirectory>{Environment.NewLine}" +
                "    </Content>";
        
        return content.Replace(AppSettingsPlaceholder, replaceContent);
    }
}