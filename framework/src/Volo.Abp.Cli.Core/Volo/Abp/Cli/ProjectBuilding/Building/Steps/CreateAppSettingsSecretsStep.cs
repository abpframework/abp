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

        var projectFiles = context.Files.Where(x => x.Content.Contains(AppSettingsPlaceholder)).ToList();

        foreach (var projectFile in projectFiles)
        {
            projectFile.SetContent(ReplaceAppSettingsSecretsPlaceholder(projectFile.Content));
        }
    }

    private static byte[] GetAppSettingsSecretJsonContent(ProjectBuildContext context)
    {
        return context.Template.IsPro()
            ? $"{{{Environment.NewLine}  \"AbpLicenseCode\": \"{CliConsts.LicenseCodePlaceHolder}\" {Environment.NewLine}}}".GetBytes()
            : $"{{{Environment.NewLine}}}".GetBytes();
    }

    private static bool NotBlazorWasmProject(string fileName)
    {
        return !fileName.Contains("Blazor/wwwroot") && !fileName.Contains("Blazor.Host/wwwroot");
    }

    private static string ReplaceAppSettingsSecretsPlaceholder(string content)
    {
        var replaceContent = $"<None Remove=\"{CliConsts.AppSettingsSecretJsonFileName}\" />{Environment.NewLine}" +
                $"    <Content Include=\"{CliConsts.AppSettingsSecretJsonFileName}\">{Environment.NewLine}" +
                $"      <CopyToPublishDirectory>PreserveNewest</CopyToPublishDirectory>{Environment.NewLine}" +
                $"      <CopyToOutputDirectory>Always</CopyToOutputDirectory>{Environment.NewLine}" +
                "    </Content>";
        return content.Replace(AppSettingsPlaceholder, replaceContent);
    }
}