# OpenIddict Blazor-Server UI Migration Guide

## Blazor Project (Non-Tiered Solution)

- In the **MyApplication.Blazor.csproj** replace **project references**:

  ```csharp
  <PackageReference Include="Volo.Abp.AspNetCore.Authentication.JwtBearer" Version="6.0.*" />
  <PackageReference Include="Volo.Abp.Account.Web.IdentityServer" Version="6.0.*" />
  ```

  with   

  ```csharp
  <PackageReference Include="Volo.Abp.Account.Web.OpenIddict" Version="6.0.*" />
  ```

- In the **MyApplicationBlazorModule.cs** replace usings and **module dependencies**:

  ```csharp
  using System;
  using System.Net.Http;
  using Volo.Abp.AspNetCore.Authentication.JwtBearer;
  ...
  typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
  typeof(AbpAccountWebIdentityServerModule),
  ```

  with 

  ```csharp
  using OpenIddict.Validation.AspNetCore;
  ...
  typeof(AbpAccountWebOpenIddictModule),
  ```

- In the **MyApplicationBlazorModule.cs** add `PreConfigureServices` like below with your application name as the audience:

  ```csharp
  public override void PreConfigureServices(ServiceConfigurationContext context)
  {
      PreConfigure<OpenIddictBuilder>(builder =>
      {
          builder.AddValidation(options =>
          {
              options.AddAudiences("MyApplication"); // Replace with your application name
              options.UseLocalServer();
              options.UseAspNetCore();
          });
      });
  }
  ```

- In the **MyApplicationBlazorModule.cs** `ConfigureServices` method, **replace the method call**:

  From `ConfigureAuthentication(context, configuration);` to `ConfigureAuthentication(context);` and update the method as:

  ```csharp
  private void ConfigureAuthentication(ServiceConfigurationContext context)
  {
      context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
  }
  ```

- In the **MyApplicationBlazorModule.cs** `OnApplicationInitialization` method, **replace the midware**:

  ```csharp
  app.UseJwtTokenMiddleware();
  app.UseIdentityServer();
  ```

  with

  ```csharp
  app.UseAbpOpenIddictValidation();
  ```

## Blazor Project (Tiered Solution)

- In the **MyApplicationWebModule.cs** update the `AddAbpOpenIdConnect` configurations:

  ```csharp
  .AddAbpOpenIdConnect("oidc", options =>
  {
  	options.Authority = configuration["AuthServer:Authority"];
      options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
      options.ResponseType = OpenIdConnectResponseType.CodeIdToken;
  
      options.ClientId = configuration["AuthServer:ClientId"];
      options.ClientSecret = configuration["AuthServer:ClientSecret"];
  
      options.SaveTokens = true;
      options.GetClaimsFromUserInfoEndpoint = true;
  
      options.Scope.Add("roles"); // Replace "role" with "roles"
      options.Scope.Add("email");
      options.Scope.Add("phone");
      options.Scope.Add("MyApplication");
  });
  ```
  
  Replace **role** scope with **roles**.

## IdentityServer

This project is renamed to **AuthServer** after v6.0.0. You can also refactor and rename your project to *AuthServer* for easier updates in the future.

- In **MyApplication.IdentityServer.csproj** replace **project references**:

  ```csharp
  <PackageReference Include="Volo.Abp.Account.Web.IdentityServer" Version="6.0.*" />
  ```

  with   

  ```csharp
  <PackageReference Include="Volo.Abp.Account.Web.OpenIddict" Version="6.0.*" />
  ```

- In **MyApplicationIdentityServerModule.cs** replace usings and **module dependencies**:

  ```csharp
  typeof(AbpAccountWebIdentityServerModule),
  ```

  with 

  ```csharp
  typeof(AbpAccountWebOpenIddictModule),
  ```

- In the **MyApplicationIdentityServerModule.cs** add `PreConfigureServices` like below with your application name as the audience:

  ```csharp
  public override void PreConfigureServices(ServiceConfigurationContext context)
  {
      PreConfigure<OpenIddictBuilder>(builder =>
      {
          builder.AddValidation(options =>
          {
              options.AddAudiences("MyApplication"); // Replace with your application name
              options.UseLocalServer();
              options.UseAspNetCore();
          });
      });
  }
  ```

- In **MyApplicationIdentityServerModule.cs** `OnApplicationInitialization` method **remove IdentityServer midware**:

  ```csharp
  app.UseIdentityServer();
  ```

## Http.Api.Host

- In the **MyApplicationHttpApiHostModule.cs** `OnApplicationInitialization` method, delete `c.OAuthClientSecret(configuration["AuthServer:SwaggerClientSecret"]);` in `app.UseAbpSwaggerUI` options configurations which is no longer needed.

- In `appsettings.json` delete **SwaggerClientSecret** from the *AuthServer* section like below:

```json
"AuthServer": {
  "Authority": "https://localhost:44345",
  "RequireHttpsMetadata": "false",
  "SwaggerClientId": "MyApplication_Swagger"
},
```

- To use the new AuthServer page, replace **Index.cshtml.cs** with [AuthServer Index.cshtml.cs](https://github.com/abpframework/abp-samples/blob/master/Ids2OpenId/src/Ids2OpenId.IdentityServer/Pages/Index.cshtml) and **Index.cshtml** file with [AuthServer Index.cshtml](https://github.com/abpframework/abp-samples/blob/master/Ids2OpenId/src/Ids2OpenId.IdentityServer/Pages/Index.cshtml.cs) and rename **Ids2OpenId** with your application namespace.

  > Note: It can be found under the *Pages* folder.

## See Also

* [OpenIddict Step-by-Step Guide](OpenIddict-Step-by-Step.md)
