# Hangfire Background Job Manager

[Hangfire](https://www.hangfire.io/) is an advanced background job manager. You can integrate Hangfire with the ABP Framework to use it instead of the [default background job manager](Background-Jobs.md). In this way, you can use the same background job API for Hangfire and your code will be independent of Hangfire. If you like, you can directly use Hangfire's API, too.

> See the [background jobs document](Background-Jobs.md) to learn how to use the background job system. This document only shows how to install and configure the Hangfire integration.

## Installation

It is suggested to use the [ABP CLI](CLI.md) to install this package.

### Using the ABP CLI

Open a command line window in the folder of the project (.csproj file) and type the following command:

````bash
abp add-package Volo.Abp.BackgroundJobs.HangFire
````

### Manual Installation

If you want to manually install;

1. Add the [Volo.Abp.BackgroundJobs.HangFire](https://www.nuget.org/packages/Volo.Abp.BackgroundJobs.HangFire) NuGet package to your project:

   ````
   Install-Package Volo.Abp.BackgroundJobs.HangFire
   ````

2. Add the `AbpBackgroundJobsHangfireModule` to the dependency list of your module:

````csharp
[DependsOn(
    //...other dependencies
    typeof(AbpBackgroundJobsHangfireModule) //Add the new module dependency
    )]
public class YourModule : AbpModule
{
}
````

## Configuration

You can install any storage for Hangfire. The most common one is SQL Server (see the [Hangfire.SqlServer](https://www.nuget.org/packages/Hangfire.SqlServer) NuGet package).

After you have installed these NuGet packages, you need to configure your project to use Hangfire.

1.First, we change the `Module` class (example: `<YourProjectName>HttpApiHostModule`) to add Hangfire configuration of the storage and connection string in the `ConfigureServices` method:

````csharp
  public override void ConfigureServices(ServiceConfigurationContext context)
  {
      var configuration = context.Services.GetConfiguration();
      var hostingEnvironment = context.Services.GetHostingEnvironment();

      //... other configarations.

      ConfigureHangfire(context, configuration);
  }

  private void ConfigureHangfire(ServiceConfigurationContext context, IConfiguration configuration)
  {
      context.Services.AddHangfire(config =>
      {
          config.UseSqlServerStorage(configuration.GetConnectionString("Default"));
      });
  }
````

2. If you want to use hangfire's dashboard, you can add `UseHangfireDashboard` call in the `OnApplicationInitialization` method in `Module` class

````csharp
 public override void OnApplicationInitialization(ApplicationInitializationContext context)
 {
    var app = context.GetApplicationBuilder();
            
    // ... others
    
    app.UseHangfireDashboard();
 
 }
````

### Dashboard Authorization

Hangfire can show a **dashboard page** so you can see the status of all background
jobs in real time. You can configure it as described in its
[documentation](http://docs.hangfire.io/en/latest/configuration/using-dashboard.html).
By default, this dashboard page is available for all users, and is not
authorized. You can integrate it in to ABP's [authorization
system](Authorization.md) using the **AbpHangfireAuthorizationFilter**
class defined in the Volo.Abp.Hangfire package. Example configuration:

    app.UseHangfireDashboard("/hangfire", new DashboardOptions
    {
        AsyncAuthorization = new[] { new AbpHangfireAuthorizationFilter() }
    });

This checks if the current user has logged in to the application. If you
want to require an additional permission, you can pass into its
constructor:

    app.UseHangfireDashboard("/hangfire", new DashboardOptions
    {
        AsyncAuthorization = new[] { new AbpHangfireAuthorizationFilter("MyHangFireDashboardPermissionName") }
    });

**Note**: UseHangfireDashboard should be called after the authentication
middleware in your Startup class (probably as the last line). Otherwise,
authorization will always fail.
