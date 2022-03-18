using System.Collections.Generic;
using System.Security.Policy;
using Volo.Abp.Cli.ProjectBuilding.Templates.App;
using Volo.Abp.Cli.ProjectBuilding.Templates.Microservice;
using Volo.Abp.Cli.ProjectBuilding.Templates.Module;
using Volo.Abp.Cli.ProjectBuilding.Templates.MvcModule;

namespace Volo.Abp.Cli;

public static class CliConsts
{
    public const string Command = "AbpCliCommand";

    public const string BranchPrefix = "branch@";

    public const string DocsLink = "https://docs.abp.io";

    public const string HttpClientName = "AbpHttpClient";

    public const string LogoutUrl = CliUrls.WwwAbpIo + "api/license/logout";

    public const string LicenseCodePlaceHolder = @"<LICENSE_CODE/>";

    public const string AppSettingsJsonFileName = "appsettings.json";

    public const string AppSettingsSecretJsonFileName = "appsettings.secrets.json";
}
