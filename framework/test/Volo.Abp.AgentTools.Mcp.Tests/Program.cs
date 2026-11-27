using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Volo.Abp.AgentTools.Mcp;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
// WebApplicationFactory hands a test's UseSetting values over as arguments.
const string argument = $"--{Program.StartupModuleSetting}=";
await (args.FirstOrDefault(a => a.StartsWith(argument, StringComparison.Ordinal))?[argument.Length..] switch
{
    null => builder.RunAbpModuleAsync<AgentToolsMcpTestModule>(),
    nameof(AttributeToolsBeforeModule) => builder.RunAbpModuleAsync<AttributeToolsBeforeModule>(),
    nameof(AttributeToolsAfterModule) => builder.RunAbpModuleAsync<AttributeToolsAfterModule>(),
    nameof(OwnHandlersBeforeModule) => builder.RunAbpModuleAsync<OwnHandlersBeforeModule>(),
    nameof(OwnHandlersAfterModule) => builder.RunAbpModuleAsync<OwnHandlersAfterModule>(),
    var module => throw new ArgumentException(module)
});

public partial class Program
{
    /// <summary>The startup module of the host; not set = <see cref="AgentToolsMcpTestModule"/>.</summary>
    public const string StartupModuleSetting = "StartupModule";
}
