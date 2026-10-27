using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.OpenIddict.Integration;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    EnvironmentName = Environments.Staging
});

await builder.RunAbpModuleAsync<OpenIddictTokenIntegrationTestModule>();

public partial class Program
{
}
