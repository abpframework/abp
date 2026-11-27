using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.OpenIddict.AgentTools;

var builder = WebApplication.CreateBuilder();
// WebApplicationFactory hands a test's UseSetting values over as arguments.
builder.Configuration.AddCommandLine(
    args.Where(a => a.StartsWith($"--{AgentToolsOpenIddictTestModule.AcceptAnyAudienceSetting}=", StringComparison.Ordinal)).ToArray());
await builder.RunAbpModuleAsync<AgentToolsOpenIddictTestModule>();

public partial class Program
{
}
