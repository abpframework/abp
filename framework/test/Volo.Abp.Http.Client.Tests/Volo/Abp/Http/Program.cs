using Microsoft.AspNetCore.Builder;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Http;

var builder = WebApplication.CreateBuilder();
await builder.RunAbpModuleAsync<AbpHttpClientTestModule>();

public partial class Program
{
}
