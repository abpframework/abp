using Microsoft.AspNetCore.Builder;
using Volo.Abp.AspNetCore;
using Volo.Abp.AspNetCore.Mvc.Versioning;

var builder = WebApplication.CreateBuilder();
await builder.RunAbpModuleAsync<AbpAspNetCoreMvcVersioningTestModule>();

public partial class Program
{
}
