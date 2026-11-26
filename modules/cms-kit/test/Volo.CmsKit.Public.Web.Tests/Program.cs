using Microsoft.AspNetCore.Builder;
using Volo.Abp.AspNetCore.TestBase;
using Volo.CmsKit.PublicWebTests;

var builder = WebApplication.CreateBuilder(args);
await builder.RunAbpModuleAsync<CmsKitPublicWebTestModule>();

public partial class CmsKitPublicWebTestProgram
{
}
