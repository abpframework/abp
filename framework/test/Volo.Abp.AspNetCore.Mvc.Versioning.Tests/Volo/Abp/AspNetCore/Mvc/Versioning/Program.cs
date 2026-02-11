using Microsoft.AspNetCore.Builder;
using Volo.Abp.AspNetCore.Mvc.Versioning;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
await builder.RunAbpModuleAsync<AbpAspNetCoreMvcVersioningTestModule>();

public partial class Program
{
}
