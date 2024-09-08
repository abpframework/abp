# Hangfire Background Job Manager

[Hangfire](https://www.hangfire.io/) is an advanced background job manager. You can integrate Hangfire with the ABP to use it instead of the [default background job manager](../background-jobs). In this way, you can use the same background job API for Hangfire and your code will be independent of Hangfire. If you like, you can directly use Hangfire's API, too.

> See the [background jobs document](../background-jobs) to learn how to use the background job system. This document only shows how to install and configure the Hangfire integration.

## Installation

It is suggested to use the [ABP CLI](../../../cli) to install this package.

### Using the ABP CLI

Open a command line window in the folder of the project (.csproj file) and type the following command:

````bash
abp add-package Volo.Abp.BackgroundJobs.HangFire
````

>  If you haven't done it yet, you first need to install the [ABP CLI](../../../cli). For other installation options, see [the package description page](https://abp.io/package-detail/Volo.Abp.BackgroundJobs.HangFire).

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

      //... other configurations.

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

> You have to configure a storage for Hangfire.

2. If you want to use hangfire's dashboard, you can add `UseAbpHangfireDashboard` call in the `OnApplicationInitialization` method in `Module` class:

````csharp
 public override void OnApplicationInitialization(ApplicationInitializationContext context)
 {
    var app = context.GetApplicationBuilder();
            
    // ... others
    
    app.UseAbpHangfireDashboard(); //should add to the request pipeline before the app.UseConfiguredEndpoints()
    app.UseConfiguredEndpoints();
 }
````

### Specifying Queue

You can use the [`QueueAttribute`](https://docs.hangfire.io/en/latest/background-processing/configuring-queues.html) to specify the queue:

````csharp
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Emailing;

namespace MyProject
{
    [Queue("alpha")]
    public class EmailSendingJob
        : AsyncBackgroundJob<EmailSendingArgs>, ITransientDependency
    {
        private readonly IEmailSender _emailSender;

        public EmailSendingJob(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public override async Task ExecuteAsync(EmailSendingArgs args)
        {
            await _emailSender.SendAsync(
                args.EmailAddress,
                args.Subject,
                args.Body
            );
        }
    }
}
````

### Dashboard Authorization

Hangfire Dashboard provides information about your background jobs, including method names and serialized arguments as well as gives you an opportunity to manage them by performing different actions – retry, delete, trigger, etc. So it is important to restrict access to the Dashboard.
To make it secure by default, only local requests are allowed, however you can change this by following the [official documentation](http://docs.hangfire.io/en/latest/configuration/using-dashboard.html) of Hangfire.

You can integrate the Hangfire dashboard to [ABP authorization system](../../fundamentals/authorization.md) using the **AbpHangfireAuthorizationFilter**
class. This class is defined in the `Volo.Abp.Hangfire` package. The following example, checks if the current user is logged in to the application:

```csharp
app.UseAbpHangfireDashboard("/hangfire", options =>
{
    options.AsyncAuthorization = new[] { new AbpHangfireAuthorizationFilter() };
});
```

* `AbpHangfireAuthorizationFilter` is an implementation of an authorization filter.

#### AbpHangfireAuthorizationFilter

`AbpHangfireAuthorizationFilter` class has the following fields:

* **`enableTenant`  (`bool`, default: `false`):** Enables/disables accessing the Hangfire dashboard on tenant users.
* **`requiredPermissionName`  (`string`, default: `null`):** Hangfire dashboard is accessible only if the current user has the specified permission. In this case, if we specify a permission name, we don't need to set `enableTenant` `true` because the permission system already does it.

If you want to require an additional permission, you can pass it into the constructor as below:

```csharp
app.UseAbpHangfireDashboard("/hangfire", options =>
{
    options.AsyncAuthorization = new[] { new AbpHangfireAuthorizationFilter(requiredPermissionName: "MyHangFireDashboardPermissionName") };
});
```

**Important**: `UseAbpHangfireDashboard` should be called after the authentication and authorization middlewares in your `Startup` class (probably at the last line). Otherwise,
authorization will always fail!

#### Dashboard Authorization In API Projects

If you use the hangfire dashboard in an API project that uses non-cookie authentication (like JWT Bearers), The `/hangfire` page can't authenticate the user.

In this case, you can add a cookies authorization scheme to authenticate the user. The best way to do this is to use the `Cookie` and `OpenIdConnect` authentication schemes. This requires creating a new OAuth2 client and adding the `ClientId`, and `ClientSecret` properties to the `AuthServer` section in the `appsettings.json` file.

The final code should look like below:

```csharp
private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
{
    context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddAbpJwtBearer(options =>
        {
            options.Authority = configuration["AuthServer:Authority"];
            options.RequireHttpsMetadata = configuration.GetValue<bool>("AuthServer:RequireHttpsMetadata");
            options.Audience = "MyProjectName";
        });

    context.Services.AddAuthentication()
        .AddCookie("Cookies")
        .AddOpenIdConnect("oidc", options =>
        {
            options.Authority = configuration["AuthServer:Authority"];
            options.RequireHttpsMetadata = configuration.GetValue<bool>("AuthServer:RequireHttpsMetadata");
            options.ResponseType = OpenIdConnectResponseType.CodeIdToken;

            options.ClientId = configuration["AuthServer:ClientId"];
            options.ClientSecret = configuration["AuthServer:ClientSecret"];

            options.UsePkce = true;
            options.SaveTokens = true;
            options.GetClaimsFromUserInfoEndpoint = true;

            options.Scope.Add("roles");
            options.Scope.Add("email");
            options.Scope.Add("phone");
            options.Scope.Add("MyProjectName");
        });
}
```

```csharp
app.Use(async (httpContext, next) =>
{
    if (httpContext.Request.Path.StartsWithSegments("/hangfire"))
    {
        var result = await httpContext.AuthenticateAsync("Cookies");
        if (result.Succeeded)
        {
            httpContext.User = result.Principal;
            await next(httpContext);
            return;
        }

        await httpContext.ChallengeAsync("oidc");
    }
    else
    {
        await next(httpContext);
    }
});

app.UseAbpHangfireDashboard("/hangfire", options =>
{
    options.AsyncAuthorization = new[] {new AbpHangfireAuthorizationFilter()};
});
```
