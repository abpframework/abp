# Using Hangfire Dashboard in ABP API Website 🚀

## Introduction

In this article, I'll show you how to integrate and use the Hangfire Dashboard in an ABP API website. 

Typically, API websites use `JWT Bearer` authentication, but the Hangfire Dashboard isn't compatible with `JWT Bearer` authentication. Therefore, we need to implement `Cookies` and `OpenIdConnect` authentication for the Hangfire Dashboard access.

## Creating a New ABP Demo Project 🛠️

We'll create a new ABP Demo `Tiered` project that includes `AuthServer`, `API`, and `Web` projects.

```bash
abp new AbpHangfireDemoApp -t app --tiered
```

Now let's add the Hangfire Dashboard to the `API` project and configure it to use `Cookies` and `OpenIdConnect` authentication for accessing the dashboard.

## Adding a New Hangfire Application 🔧

We need to add a new Hangfire application to the `appsettings.json` file in the `DbMigrator` project:

> **Note:** Replace `44371` with your `API` project's port.

```json
"OpenIddict": {
    "Applications": {
        //...
        "AbpHangfireDemoApp_Hangfire": {
            "ClientId": "AbpHangfireDemoApp_Hangfire",
            "RootUrl": "https://localhost:44371/"
        }
        //...
    }
}
```

2. Update the `OpenIddictDataSeedContributor`'s `CreateApplicationsAsync` method in the `Domain` project to seed the new Hangfire application.

```csharp
 //Hangfire Client
var hangfireClientId = configurationSection["AbpHangfireDemoApp_Hangfire:ClientId"];
if (!hangfireClientId.IsNullOrWhiteSpace())
{
    var hangfireClientRootUrl = configurationSection["AbpHangfireDemoApp_Hangfire:RootUrl"]!.EnsureEndsWith('/');

    await CreateApplicationAsync(
        applicationType: OpenIddictConstants.ApplicationTypes.Web,
        name: hangfireClientId!,
        type: OpenIddictConstants.ClientTypes.Confidential,
        consentType: OpenIddictConstants.ConsentTypes.Implicit,
        displayName: "Hangfire Application",
        secret: configurationSection["AbpHangfireDemoApp_Hangfire:ClientSecret"] ?? "1q2w3e*",
        grantTypes: new List<string> //Hybrid flow
        {
            OpenIddictConstants.GrantTypes.AuthorizationCode, OpenIddictConstants.GrantTypes.Implicit
        },
        scopes: commonScopes,
        redirectUris: new List<string> { $"{hangfireClientRootUrl}signin-oidc" },
        postLogoutRedirectUris: new List<string> { $"{hangfireClientRootUrl}signout-callback-oidc" },
        clientUri: hangfireClientRootUrl,
        logoUri: "/images/clients/aspnetcore.svg"
    );
}
```

3. Run the `DbMigrator` project to seed the new Hangfire application.

### Adding Hangfire Dashboard to the `API` Project 📦

1. Add the following packages and modules dependencies to the `API` project:

```bash
<PackageReference Include="Volo.Abp.BackgroundJobs.HangFire" Version="9.2.0" />
<PackageReference Include="Volo.Abp.AspNetCore.Authentication.OpenIdConnect" Version="9.2.0" />
<PackageReference Include="Hangfire.SqlServer" Version="1.8.20" />
```

```cs
typeof(AbpBackgroundJobsHangfireModule),
typeof(AbpAspNetCoreAuthenticationOpenIdConnectModule)
```

2. Add the `HangfireClientId` and `HangfireClientSecret` to the `appsettings.json` file in the `API` project:

```csharp
"AuthServer": {
    "Authority": "https://localhost:44358",
    "RequireHttpsMetadata": true,
    "MetaAddress": "https://localhost:44358",
    "SwaggerClientId": "AbpHangfireDemoApp_Swagger",
    "HangfireClientId": "AbpHangfireDemoApp_Hangfire",
    "HangfireClientSecret": "1q2w3e*"
}
```

3. Add the `ConfigureHangfire` method to the `API` project to configure Hangfire:

```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    var configuration = context.Services.GetConfiguration();
    var hostingEnvironment = context.Services.GetHostingEnvironment();

    //...

    //Add Hangfire
    ConfigureHangfire(context, configuration);
    //...
}

private void ConfigureHangfire(ServiceConfigurationContext context, IConfiguration configuration)
{
    context.Services.AddHangfire(config =>
    {
        config.UseSqlServerStorage(configuration.GetConnectionString("Default"));
    });
}
```

4. Modify the `ConfigureAuthentication` method to add new `Cookies` and `OpenIdConnect` authentication schemes:

```csharp
private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
{
    context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddAbpJwtBearer(options =>
        {
            options.Authority = configuration["AuthServer:Authority"];
            options.RequireHttpsMetadata = configuration.GetValue<bool>("AuthServer:RequireHttpsMetadata");
            options.Audience = "AbpHangfireDemoApp";

            options.ForwardDefaultSelector = httpContext => httpContext.Request.Path.StartsWithSegments("/hangfire", StringComparison.OrdinalIgnoreCase)
                ? CookieAuthenticationDefaults.AuthenticationScheme
                : null;
        })
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddAbpOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
        {
            options.Authority = configuration["AuthServer:Authority"];
            options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
            options.ResponseType = OpenIdConnectResponseType.Code;

            options.ClientId = configuration["AuthServer:HangfireClientId"];
            options.ClientSecret = configuration["AuthServer:HangfireClientSecret"];

            options.UsePkce = true;
            options.SaveTokens = true;
            options.GetClaimsFromUserInfoEndpoint = true;

            options.Scope.Add("roles");
            options.Scope.Add("email");
            options.Scope.Add("phone");
            options.Scope.Add("AbpHangfireDemoApp");

            options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        });

    //...
}
```

5. Add a custom middleware and `UseAbpHangfireDashboard` after `UseAuthorization` in the `OnApplicationInitialization` method:

```csharp
//...
app.UseAuthorization();

app.Use(async (httpContext, next) =>
{
    if (httpContext.Request.Path.StartsWithSegments("/hangfire", StringComparison.OrdinalIgnoreCase))
    {
        var authenticateResult = await httpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        if (!authenticateResult.Succeeded)
        {
            await httpContext.ChallengeAsync(
                OpenIdConnectDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = httpContext.Request.Path + httpContext.Request.QueryString
                });
            return;
        }
    }
    await next.Invoke();
});
app.UseAbpHangfireDashboard("/hangfire", options =>
{
    options.AsyncAuthorization = new[]
    {
        new AbpHangfireAuthorizationFilter()
    };
});

//...
```

Perfect! 🎉 Now you can run the `AuthServer` and `API` projects and access the Hangfire Dashboard at `https://localhost:44371/hangfire`.

> **Note:** Replace `44371` with your `API` project's port.

The first time you access the Hangfire Dashboard, you'll be redirected to the login page of the `AuthServer` project. After you log in, you'll be redirected back to the Hangfire Dashboard.

![Hangfire Dashboard](gif.gif)

## Key Points 🔑

### 1. Authentication Scheme Selection

The default authentication scheme in API websites is `JWT Bearer`. We've implemented `Cookies` and `OpenIdConnect` specifically for the Hangfire Dashboard.

We've configured the `JwtBearerOptions`'s `ForwardDefaultSelector` to use `CookieAuthenticationDefaults.AuthenticationScheme` for Hangfire Dashboard requests.

This means that if the request path starts with `/hangfire`, the request will be authenticated using the `Cookies` authentication scheme; otherwise, it will use the `JwtBearer` authentication scheme.

```csharp
options.ForwardDefaultSelector = httpContext => httpContext.Request.Path.StartsWithSegments("/hangfire", StringComparison.OrdinalIgnoreCase)
    ? CookieAuthenticationDefaults.AuthenticationScheme
    : null;
```

### 2. Custom Middleware for Authentication

We've also implemented a custom middleware to handle `Cookies` authentication for the Hangfire Dashboard. If the current request isn't authenticated with the `Cookies` authentication scheme, it will be redirected to the login page.

```csharp
app.Use(async (httpContext, next) =>
{
    if (httpContext.Request.Path.StartsWithSegments("/hangfire", StringComparison.OrdinalIgnoreCase))
    {
        var authenticateResult = await httpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        if (!authenticateResult.Succeeded)
        {
            await httpContext.ChallengeAsync(
                OpenIdConnectDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = httpContext.Request.Path + httpContext.Request.QueryString
                });
            return;
        }
    }
    await next.Invoke();
});
```

## References 📚

- [ABP Hangfire Background Job Manager](https://abp.io/docs/latest/framework/infrastructure/background-jobs/hangfire)
- [Use cookie authentication in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-9.0)
