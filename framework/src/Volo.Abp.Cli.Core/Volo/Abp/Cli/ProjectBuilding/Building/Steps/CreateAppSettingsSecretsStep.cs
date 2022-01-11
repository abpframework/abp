using System;
using System.Linq;
using Volo.Abp.Cli.ProjectBuilding.Files;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps;

public class CreateAppSettingsSecretsStep : ProjectBuildPipelineStep
{
    private const string FileName = "appsettings.secrets.json";
    private const string AppSettingsFileName = "appsettings.json";
    private const string AppSettingsPlaceholder = "<!--APPSETTINGS-SECRETS-->";

    public override void Execute(ProjectBuildContext context)
    {
        var appSettingsFiles = context.Files
            .Where(x =>
                x.Name.EndsWith(AppSettingsFileName) &&
                NotBlazorWasmProject(x.Name))
            .ToList();

        var content = context.Template.IsPro()
            ? $"{{{Environment.NewLine}  \"AbpLicenseCode\": \"<LICENSE_CODE/>\" {Environment.NewLine}}}"
            : $"{{{Environment.NewLine}}}";

        foreach (var appSettingsFile in appSettingsFiles)
        {
            context.Files.Add(new FileEntry(
                appSettingsFile.Name.Replace(AppSettingsFileName, FileName),
                content.GetBytes(),
                false));
        }

        var projectFiles = context.Files.Where(x => x.Content.Contains(AppSettingsPlaceholder)).ToList();

        foreach (var projectFile in projectFiles)
        {
            projectFile.SetContent(ReplaceAppSettingsSecretsPlaceholder(projectFile.Content));
        }
    }

    private static bool NotBlazorWasmProject(string fileName)
    {
        return !fileName.Contains("Blazor/wwwroot") && !fileName.Contains("Blazor.Host/wwwroot");
    }

    private static string ReplaceAppSettingsSecretsPlaceholder(string content)
    {
        var replaceContent = $"<None Remove=\"{FileName}\" />{Environment.NewLine}" +
                $"    <Content Include=\"{FileName}\">{Environment.NewLine}" +
                $"      <CopyToPublishDirectory>PreserveNewest</CopyToPublishDirectory>{Environment.NewLine}" +
                $"      <CopyToOutputDirectory>Always</CopyToOutputDirectory>{Environment.NewLine}" +
                "    </Content>";
        return content.Replace(AppSettingsPlaceholder, replaceContent);
    }
}
