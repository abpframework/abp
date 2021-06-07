using System;
using System.Linq;
using Volo.Abp.Cli.ProjectBuilding.Files;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps
{
    public class CreateAppSettingsSecretsStep : ProjectBuildPipelineStep
    {
        private const string FileName = "appsettings.secrets.json";
        private const string AppSettingsFileName = "appsettings.json";

        public override void Execute(ProjectBuildContext context)
        {
            var appSettingsFiles = context.Files
                .Where(x =>
                    x.Name.EndsWith(AppSettingsFileName) &&
                    NotTestProject(x.Name) &&
                    NotBlazorWasmProject(x.Name) &&
                    NotMigratorProject(x.Name))
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
        }

        private static bool NotTestProject(string fileName)
        {
            return !fileName.StartsWith("/aspnet-core/test");
        }

        private static bool NotBlazorWasmProject(string fileName)
        {
            return !fileName.Contains("Blazor/wwwroot") && !fileName.Contains("Blazor.Host/wwwroot");
        }

        public static bool NotMigratorProject(string fileName)
        {
            return !fileName.Contains("DbMigrator");
        }
    }
}
