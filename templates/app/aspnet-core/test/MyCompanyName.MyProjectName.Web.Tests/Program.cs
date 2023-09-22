using Microsoft.AspNetCore.Builder;
using MyCompanyName.MyProjectName;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
await builder.RunAbpModuleAsync<MyProjectNameWebTestModule>();

public partial class Program
{
}
