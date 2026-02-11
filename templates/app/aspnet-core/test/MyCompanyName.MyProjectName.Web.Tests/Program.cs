using Microsoft.AspNetCore.Builder;
using MyCompanyName.MyProjectName;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();

builder.Environment.ContentRootPath = GetWebProjectContentRootPathHelper.Get("MyCompanyName.MyProjectName.Web.csproj");
await builder.RunAbpModuleAsync<MyProjectNameWebTestModule>(applicationName: "MyCompanyName.MyProjectName.Web" );

public partial class Program
{
}
