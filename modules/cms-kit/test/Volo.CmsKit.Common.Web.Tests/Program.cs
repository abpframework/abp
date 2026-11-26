using Microsoft.AspNetCore.Builder;
using Volo.Abp.AspNetCore.TestBase;
using Volo.CmsKit.Web;

var builder = WebApplication.CreateBuilder(args);
await builder.RunAbpModuleAsync<CmsKitCommonWebTestModule>();

public partial class Program
{
}
