using Microsoft.AspNetCore.Builder;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
await builder.RunAbpModuleAsync<AbpAspNetCoreSignalRTestModule>();

public partial class Program
{
}
