namespace Volo.Abp.Cli.Bundling;

internal static class BundlingConsts
{
    internal const string StylePlaceholderStart = "<!--ABP:Styles-->";
    internal const string StylePlaceholderEnd = "<!--/ABP:Styles-->";
    internal const string ScriptPlaceholderStart = "<!--ABP:Scripts-->";
    internal const string ScriptPlaceholderEnd = "<!--/ABP:Scripts-->";
    internal const string SupportedWebAssemblyProjectType = "Microsoft.NET.Sdk.BlazorWebAssembly";
}
