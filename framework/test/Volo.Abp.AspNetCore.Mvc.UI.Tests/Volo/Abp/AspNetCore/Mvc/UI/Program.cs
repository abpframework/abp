using Microsoft.AspNetCore.Builder;
using Volo.Abp.AspNetCore;
using Volo.Abp.AspNetCore.Mvc.UI;

var builder = WebApplication.CreateBuilder();
await builder.RunAbpModuleAsync<AbpAspNetCoreMvcUiTestModule>();

public partial class Program
{
}
