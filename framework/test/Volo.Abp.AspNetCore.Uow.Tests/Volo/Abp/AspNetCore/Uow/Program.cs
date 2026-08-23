using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.AspNetCore.Uow;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    EnvironmentName = Environments.Staging
});

await builder.RunAbpModuleAsync<AbpAspNetCoreUowTestModule>();

public partial class Program
{
}
