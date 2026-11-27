using Microsoft.AspNetCore.Builder;
using Volo.Abp.AgentTools;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
await builder.RunAbpModuleAsync<AgentToolsAspNetCoreTestModule>();

public partial class Program
{
}
