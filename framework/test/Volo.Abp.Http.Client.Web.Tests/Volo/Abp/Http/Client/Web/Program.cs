using Microsoft.AspNetCore.Builder;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Http.Client.Web;

var builder = WebApplication.CreateBuilder();
await builder.RunAbpModuleAsync<AbpHttpClientWebTestModule>();

public partial class Program
{
}
