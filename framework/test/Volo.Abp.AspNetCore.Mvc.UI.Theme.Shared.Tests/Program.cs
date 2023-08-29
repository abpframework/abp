using Microsoft.AspNetCore.Builder;
using Volo.Abp.AspNetCore;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Tests.Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;

var builder = WebApplication.CreateBuilder();
await builder.RunAbpModuleAsync<AbpAspNetCoreMvcUiThemeSharedTestModule>();

public partial class Program
{
}
