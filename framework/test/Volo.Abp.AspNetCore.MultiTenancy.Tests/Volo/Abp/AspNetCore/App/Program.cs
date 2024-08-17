using Microsoft.AspNetCore.Builder;
using Volo.Abp.AspNetCore.App;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
await builder.RunAbpModuleAsync<AppModule>();

public partial class Program
{
}
