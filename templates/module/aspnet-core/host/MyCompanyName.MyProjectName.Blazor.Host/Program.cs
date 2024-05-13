using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyCompanyName.MyProjectName.Blazor.Host;
using MyCompanyName.MyProjectName.Blazor.Host.Client;
using Volo.Abp.AspNetCore.Components.WebAssembly.WebApp;

var builder = WebApplication.CreateBuilder(args);

//https://github.com/dotnet/aspnetcore/issues/52530
builder.Services.Configure<RouteOptions>(options =>
{
    options.SuppressCheckForUnhandledSecurityMetadata = true;
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(WebAppAdditionalAssembliesHelper.GetAssemblies<MyProjectNameBlazorHostClientModule>());

await app.RunAsync();
