using Microsoft.AspNetCore.Builder;
using Volo.Abp.AspNetCore.Mvc.UI;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
await builder.RunAbpModuleAsync<AbpAspNetCoreMvcUiTestModule>();

public partial class Program
{
}
