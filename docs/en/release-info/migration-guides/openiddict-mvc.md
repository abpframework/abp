# OpenIddict MVC/Razor UI Migration Guide

## Web Project (Non-Tiered Solution)

- In **MyApplication.Web.csproj** replace **project references**:

  ```csharp
  <PackageReference Include="Volo.Abp.AspNetCore.Authentication.JwtBearer" Version="6.0.*" />
  <PackageReference Include="Volo.Abp.Account.Web.IdentityServer" Version="6.0.*" />
  ```

  with   

  ```csharp
  <PackageReference Include="Volo.Abp.Account.Web.OpenIddict" Version="6.0.*" />
  ```

- In **MyApplicationWebModule.cs** replace usings and **module dependencies**:

  ```csharp
  using Volo.Abp.AspNetCore.Authentication.JwtBearer;
  ...
  typeof(AbpAccountWebIdentityServerModule),
  typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
  ```

  with 

  ```csharp
  typeof(AbpAccountWebOpenIddictModule),
  ```

- In **MyApplicationWebModule.cs** `ConfigureServices` method **update authentication configuration**:

  ```csharp
  ConfigureAuthentication(context, configuration);
  ```

  with

  ```csharp
  ConfigureAuthentication(context);
  ```

  and update the `ConfigureAuthentication` private method to:

  ```csharp
  private void ConfigureAuthentication(ServiceConfigurationContext context)
  {
      context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
  }
  ```
  
  - In the **MyApplicationWebModule.cs** add `PreConfigureServices` like below with your application name as the audience:

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

- In **MyApplicationWebModule.cs** `OnApplicationInitialization` method **replace IdentityServer and JwtToken midwares**:

  ```csharp
  app.UseJwtTokenMiddleware();
  app.UseIdentityServer();
  ```

  with

  ```csharp
  app.UseAbpOpenIddictValidation();
  ```


## Web Project (Tiered Solution)

- In the **MyApplicationWebModule.cs** update the `AddAbpOpenIdConnect` configurations:

  ```csharp
  .AddAbpOpenIdConnect("oidc", options =>
  {
      options.Authority = configuration["AuthServer:Authority"];
      options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
      options.ResponseType = OpenIdConnectResponseType.CodeIdToken;
  
      options.ClientId = configuration["AuthServer:ClientId"];
      options.ClientSecret = configuration["AuthServer:ClientSecret"];
  
      options.UsePkce = true; // Add this line
      options.SaveTokens = true;
      options.GetClaimsFromUserInfoEndpoint = true
  
      options.Scope.Add("roles"); // Replace "role" with "roles"
      options.Scope.Add("email");
      options.Scope.Add("phone");
      options.Scope.Add("MyApplication");
  });
  ```
  
Replace role scope to **roles** and add **UsePkce** and **SignoutScheme** options.

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

- To use the new AuthServer page, replace **Index.cshtml.cs** with [AuthServer Index.cshtml.cs](https://github.com/abpframework/abp-samples/blob/master/Ids2OpenId/src/Ids2OpenId.IdentityServer/Pages/Index.cshtml) and **Index.cshtml** file with [AuthServer Index.cshtml](https://github.com/abpframework/abp-samples/blob/master/Ids2OpenId/src/Ids2OpenId.IdentityServer/Pages/Index.cshtml.cs) and rename **Ids2OpenId** with your application namespace.

  > Note: It can be found under the *Pages* folder.

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

## See Also

* [OpenIddict Step-by-Step Guide](OpenIddict-Step-by-Step.md)
