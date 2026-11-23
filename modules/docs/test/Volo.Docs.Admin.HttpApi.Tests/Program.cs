using Microsoft.AspNetCore.Builder;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Docs.Admin;

var builder = WebApplication.CreateBuilder(args);
await builder.RunAbpModuleAsync<DocsAdminHttpApiTestModule>();

public partial class DocsHttpApiTestProgram
{
}
