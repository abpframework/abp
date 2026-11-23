using Microsoft.AspNetCore.Builder;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Docs;

var builder = WebApplication.CreateBuilder(args);
await builder.RunAbpModuleAsync<DocsWebTestModule>();

public partial class DocsWebTestProgram
{
}
