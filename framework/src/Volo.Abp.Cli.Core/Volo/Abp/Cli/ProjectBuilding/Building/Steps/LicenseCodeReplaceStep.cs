using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Cli.ProjectBuilding.Files;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps;

public class LicenseCodeReplaceStep : ProjectBuildPipelineStep
{
    public override void Execute(ProjectBuildContext context)
    {
        var licenseCode = context.BuildArgs.ExtraProperties.GetOrDefault("license-code");

        var appSettingsJsonFiles = context.Files.Where(f =>
            f.Name.EndsWith(CliConsts.AppSettingsJsonFileName, StringComparison.OrdinalIgnoreCase) ||
            f.Name.EndsWith(CliConsts.AppSettingsSecretJsonFileName, StringComparison.OrdinalIgnoreCase));

        foreach (var appSettingsJson in appSettingsJsonFiles)
        {
            appSettingsJson.ReplaceText(CliConsts.LicenseCodePlaceHolder, licenseCode);
        }
    }
}
