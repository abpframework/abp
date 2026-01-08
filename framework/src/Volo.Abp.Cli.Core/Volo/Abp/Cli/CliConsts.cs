namespace Volo.Abp.Cli;

public static class CliConsts
{
    public static string Command = "AbpCliCommand";

    public static string BranchPrefix = "branch@";

    public static string DocsLink = "https://abp.io/docs";

    public static string HttpClientName = "AbpHttpClient";

    public static string GithubHttpClientName = "GithubHttpClient";

    public static string LogoutUrl = CliUrls.AccountAbpIo + "api/license/logout";

    public static string LicenseCodePlaceHolder = @"<LICENSE_CODE/>";

    public static string AppSettingsJsonFileName = "appsettings.json";

    public static string AppSettingsSecretJsonFileName = "appsettings.secrets.json";

    public const string McpServerUrlEnvironmentVariable = "ABP_MCP_SERVER_URL";
    public const string DefaultMcpServerUrl = "https://mcp.abp.io";

    public static class MemoryKeys
    {
        public const string LatestCliVersionCheckDate = "LatestCliVersionCheckDate";
        public const string McpToolsLastFetchDate = "McpToolsLastFetchDate";
        public const string McpServerUrl = "McpServerUrl";
    }
}
