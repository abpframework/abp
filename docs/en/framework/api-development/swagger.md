# Swagger Integration

[Swagger (OpenAPI)](https://swagger.io/) is a language-agnostic specification for describing REST APIs. It allows both computers and humans to understand the capabilities of a REST API without direct access to the source code. Its main goals are to:

- Minimize the amount of work needed to connect decoupled services.
- Reduce the amount of time needed to accurately document a service.

ABP offers a prebuilt module for full Swagger integration with small configurations. 

## Installation

> This package is already installed by default with the startup template. So, most of the time, you don't need to install it manually.

If installation is needed, it is suggested to use the [ABP CLI](../../cli/index.md) to install this package.

### Using the ABP CLI

Open a command line window in the folder of the `Web` or `HttpApi.Host` project (.csproj file) and type the following command:

```bash
abp add-package Volo.Abp.Swashbuckle
```

> If you haven't done it yet, you first need to install the [ABP CLI](../../cli/index.md). For other installation options, see [the package description page](https://abp.io/package-detail/Volo.Abp.Swashbuckle).

### Manual Installation

If you want to manually install;

1. Add the [Volo.Abp.Swashbuckle](https://www.nuget.org/packages/Volo.Abp.Swashbuckle) NuGet package to your `Web` or `HttpApi.Host` project:

   `Install-Package Volo.Abp.Swashbuckle`

2. Add the `AbpSwashbuckleModule` to the dependency list of your module:

   ```csharp
   [DependsOn(
       //...other dependencies
       typeof(AbpSwashbuckleModule) // <-- Add module dependency like that
       )]
   public class YourModule : AbpModule
   {
   }
   ```

## Configuration

First, we need to use `AddAbpSwaggerGen` extension to configure Swagger in `ConfigureServices` method of our module:

```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    var services = context.Services;

    //... other configurations.

    services.AddAbpSwaggerGen(
        options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Test API", Version = "v1" });
            options.DocInclusionPredicate((docName, description) => true);
            options.CustomSchemaIds(type => type.FullName);
        }
    );
}
```

Then we can use Swagger UI by calling `UseAbpSwaggerUI` method in the `OnApplicationInitialization` method of our module:

```csharp
public override void OnApplicationInitialization(ApplicationInitializationContext context)
{
    var app = context.GetApplicationBuilder();

    //... other configurations.

    app.UseStaticFiles();

    app.UseSwagger();

    app.UseAbpSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Test API");
    });
    
    //... other configurations.
}
```

### Hide ABP Endpoints on Swagger UI

If you want to hide ABP's default endpoints, call the `HideAbpEndpoints` method in your Swagger configuration as shown in the following example:

```csharp
services.AddAbpSwaggerGen(
    options => 
    {
        //... other options
        
        //Hides ABP Related endpoints on Swagger UI
        options.HideAbpEndpoints();
    }
)
```

## Using Swagger with OAUTH

For non MVC/Tiered applications, we need to configure Swagger with OAUTH to handle authorization.  

> ABP uses OpenIddict by default. To get more information about OpenIddict, check this [documentation](../../modules/openiddict.md). 

To do that, we need to use `AddAbpSwaggerGenWithOAuth` extension to configure Swagger with OAuth issuer and scopes in `ConfigureServices` method of our module:

```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    var services = contex.Services;

    //... other configarations.

    services.AddAbpSwaggerGenWithOAuth(
        "https://localhost:44341",             // authority issuer
        new Dictionary<string, string>         //
        {                                      // scopes
            {"Test", "Test API"}               //
        },                                     //
        options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Test API", Version = "v1" });
            options.DocInclusionPredicate((docName, description) => true);
            options.CustomSchemaIds(type => type.FullName);
        }
    );
}
```

Then we can use Swagger UI by calling `UseAbpSwaggerUI` method in the `OnApplicationInitialization` method of our module:

```csharp
public override void OnApplicationInitialization(ApplicationInitializationContext context)
{
    var app = context.GetApplicationBuilder();

    //... other configurations.

    app.UseAbpSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Test API");

        var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
        options.OAuthClientId("Test_Swagger"); // clientId
        options.OAuthClientSecret("1q2w3e*");  // clientSecret
    });
    
    //... other configurations.
}
```

> Do not forget to set `OAuthClientId` and `OAuthClientSecret`.

## Using Swagger with OIDC

You may also want to configure swagger using **OpenIdConnect** instead of OAUTH. This is especially useful when you need to configure different metadata address than the issuer in cases such as when you deploy your application to kubernetes cluster or docker. In these cases, metadata address will be used in sign-in process to reach the valid authentication server discovery endpoint over the internet and use the internal network to validate the obtained token.

To do that, we need to use `AddAbpSwaggerGenWithOidc` extension to configure Swagger with OAuth issuer and scopes in `ConfigureServices` method of our module:

```csharp
context.Services.AddAbpSwaggerGenWithOidc(
    configuration["AuthServer:Authority"],
    scopes: new[] { "SwaggerDemo" },
    // "authorization_code"
    flows: new[] { AbpSwaggerOidcFlows.AuthorizationCode },
    // When deployed on K8s, should be metadata URL of the reachable DNS over internet like https://myauthserver.company.com
    discoveryEndpoint: configuration["AuthServer:Authority"],
    options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "SwaggerDemo API", Version = "v1" });
        options.DocInclusionPredicate((docName, description) => true);
        options.CustomSchemaIds(type => type.FullName);
    });
```

The `flows` is a list of default oidc flows that is supported by the oidc-provider (authserver). You can see the default supported flows below:

- `AbpSwaggerOidcFlows.AuthorizationCode`: The `"authorization_code"` flow is the **default and suggested** flow. **Doesn't require a client secret** when even there is a field for it.
-  `AbpSwaggerOidcFlows.Implicit`: The deprecated `"implicit"` flow that was used for javascript applications.
- `AbpSwaggerOidcFlows.Password`: The legacy `password` flow which is also known as Resource Ownder Password flow. You need to provide a user name, password and client secret for it.
- `AbpSwaggerOidcFlows.ClientCredentials`: The `"client_credentials"` flow that is used for server to server interactions.

You can define one or many flows which will be shown in the Authorize modal. You can set it **null which will use the default "authorization_code"** flow.

The `discoveryEndpoint` is the reachable openid-provider endpoint for the `.well-known/openid-configuration`. You can set it to **null which will use default AuthServer:Authority** appsettings configuration. If you are deploying your applications to a kubernetes cluster or docker swarm, you should to set the `discoveryEndpoint` as real DNS that should be reachable over the internet. 

> If are having problems with seeing the authorization modal, check the browser console logs and make sure you have a correct and reachable `discoveryEndpoint`
