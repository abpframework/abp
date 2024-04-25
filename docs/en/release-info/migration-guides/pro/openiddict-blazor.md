# OpenIddict Blazor Wasm UI Migration Guide

## Blazor Project

- In the **MyApplication.Blazor.csproj** replace **project references**:

  ```csharp
  <PackageReference Include="Volo.Abp.IdentityServer.Blazor.WebAssembly" Version="6.0.*" />
  ```
  
  with   
  
  ```csharp
  <PackageReference Include="Volo.Abp.OpenIddict.Pro.Blazor.WebAssembly" Version="6.0.*" />
  ```
  
- In the **MyApplicationBlazorModule.cs** replace usings and **module dependencies**:

  ```csharp
  using IdentityModel;
  using Volo.Abp.IdentityServer.Blazor.WebAssembly;
  ...
  typeof(AbpIdentityServerBlazorWebAssemblyModule),
  ```

  with 

  ```csharp
  using OpenIddict.Abstractions;
  using Volo.Abp.OpenIddict.Pro.Blazor.WebAssembly;
  ...
  typeof(AbpOpenIddictProBlazorWebAssemblyModule),
  ```

- In the **MyApplicationBlazorModule.cs** update the `ConfigureAuthentication` method:

  ```csharp
  builder.Services.AddOidcAuthentication(options =>
  {
  	builder.Configuration.Bind("AuthServer", options.ProviderOptions);
      options.UserOptions.NameClaim = OpenIddictConstants.Claims.Name; // Add this line
      options.UserOptions.RoleClaim = OpenIddictConstants.Claims.Role; // Add this line
  
      options.ProviderOptions.DefaultScopes.Add("MyApplication");
      options.ProviderOptions.DefaultScopes.Add("roles"); // Update role to roles
      options.ProviderOptions.DefaultScopes.Add("email");
      options.ProviderOptions.DefaultScopes.Add("phone");
  });
  ```
  
  Remove `options.UserOptions.RoleClaim = JwtClaimTypes.Role;` and update `role` scope to `roles`.
  
- In the **MyApplicationMenuContributor.cs** under *Navigation* folder, **replace the using and menu name** under `ConfigureMainMenuAsync`:

  ```csharp
  using Volo.Abp.IdentityServer.Blazor.Navigation;
  ...
  //Administration->Identity Server
  administration.SetSubItemOrder(AbpIdentityServerMenuNames.GroupName, 2);
  ```
  
  with 
  
  ```csharp
  using Volo.Abp.OpenIddict.Pro.Blazor.Menus;
  ...
  //Administration->OpenIddict
  administration.SetSubItemOrder(OpenIddictProMenus.GroupName, 2);
  ```

## Http.Api.Host (Non-Separated IdentityServer)

- In the **MyApplication.HttpApi.Host.csproj** replace **project references**:

  ```csharp
  <PackageReference Include="Volo.Abp.Account.Pro.Public.Web.IdentityServer" Version="6.0.*" />
  ```
  
  with   
  
  ```csharp
  <PackageReference Include="Volo.Abp.Account.Pro.Public.Web.OpenIddict" Version="6.0.*" />
  ```
  
- In the **MyApplicationHttpApiHostModule.cs** replace usings and **module dependencies**:

  ```csharp
  using IdentityServer4.Configuration;
  using Volo.Abp.AspNetCore.Authentication.JwtBearer;
  ...
  typeof(AbpAccountPublicWebIdentityServerModule),
  ```
  
  with 
  
  ```csharp
  using OpenIddict.Validation.AspNetCore;
  ...
  typeof(AbpAccountPublicWebOpenIddictModule),
  ```
  
- In the **MyApplicationHostModule.cs** add `PreConfigureServices` like below with your application name as the audience:

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

- In the **MyApplicationHostModule.cs** `ConfigureServices` method, **replace the method call**:

  From `ConfigureAuthentication(context, configuration);` to `ConfigureAuthentication(context);` and update the method as:

  ```csharp
  private void ConfigureAuthentication(ServiceConfigurationContext context)
  {
      context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
  }
  ```

- In the **MyApplicationHostModule.cs** `OnApplicationInitialization` method, **replace the midware**:

  ```csharp
  app.UseJwtTokenMiddleware();
  app.UseIdentityServer();
  ```

  with

  ```
  app.UseAbpOpenIddictValidation();
  ```

- Delete `c.OAuthClientSecret(configuration["AuthServer:SwaggerClientSecret"]);` in `app.UseAbpSwaggerUI` options configurations which is no longer needed.

- In `appsettings.json` delete **SwaggerClientSecret** from the *AuthServer* section like below:

  ```json
  "AuthServer": {
      "Authority": "https://localhost:44345",
      "RequireHttpsMetadata": "false",
      "SwaggerClientId": "MyApplication_Swagger"
  },
  ```

## Http.Api.Host (Separated IdentityServer)

- In the **MyApplicationHttpApiHostModule.cs** `OnApplicationInitialization` method, delete `c.OAuthClientSecret(configuration["AuthServer:SwaggerClientSecret"]);` in `app.UseAbpSwaggerUI` options configurations which is no longer needed.

- In `appsettings.json` delete **SwaggerClientSecret** from the *AuthServer* section like below:

  ```json
  "AuthServer": {
      "Authority": "https://localhost:44345",
      "RequireHttpsMetadata": "false",
      "SwaggerClientId": "MyApplication_Swagger"
  },
  ```

## IdentityServer

This project is renamed to **AuthServer** after v6.0.0. You can also refactor and rename your project to *AuthServer* for easier updates in the future. 

- In **MyApplication.IdentityServer.csproj** replace **project references**:

  ```csharp
  <PackageReference Include="Volo.Abp.Account.Pro.Public.Web.IdentityServer" Version="6.0.*" />
  ```

  with   

  ```csharp
  <PackageReference Include="Volo.Abp.Account.Pro.Public.Web.OpenIddict" Version="6.0.*" />
  ```

- In the **MyApplicationIdentityServerModule.cs** replace usings and **module dependencies**:

  ```csharp
  using IdentityServer4.Configuration;
  using Volo.Abp.AspNetCore.Authentication.JwtBearer;
  ...
  typeof(AbpAccountPublicWebIdentityServerModule),
  ```

  with 

  ```csharp
  using OpenIddict.Validation.AspNetCore;
  ...
  typeof(AbpAccountPublicWebOpenIddictModule),
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

- In the **MyApplicationIdentityServerModule.cs** replace **ForwardIdentityAuthenticationForBearer** under `ConfigureServices` method:

  ```csharp
  context.Services.ForwardIdentityAuthenticationForBearer();
  ```
  
  with 
  
  ```csharp
  context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
  ```
  
- In the **MyApplicationIdentityServerModule.cs**, **remove IdentityServerOptions** configuration and **JwtBearer** options under `ConfigureServices` method:

  ```csharp
  if (Convert.ToBoolean(configuration["AuthServer:SetSelfAsIssuer"])) // Remove
  {
      Configure<IdentityServerOptions>(options => { options.IssuerUri = configuration["App:SelfUrl"]; });
  }
  ...
  context.Services.AddAuthentication() // Remove
      .AddJwtBearer(options =>
      {
         options.Authority = configuration["AuthServer:Authority"];
         options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
         options.Audience = "MyApplication";
      })
  ```
  
- In the **MyApplicationIdentityServerModule.cs** `OnApplicationInitialization` method, **replace the midware**:

  ```csharp
  app.UseJwtTokenMiddleware();
  app.UseIdentityServer();
  ```
  
  with
  
 ```csharp
  app.UseAbpOpenIddictValidation();
  ```

- To use the new AuthServer page, replace **Index.cshtml.cs** with [AuthServer Index.cshtml.cs](https://gist.github.com/gterdem/878ea99edaf998bb9f360bbf1a674a87#file-index-cshtml-cs) and **Index.cshtml** file with [AuthServer Index.cshtml](https://gist.github.com/gterdem/878ea99edaf998bb9f360bbf1a674a87#file-index-cshtml). 

  > Note: It can be found under the *Pages* folder.

## See Also

* [OpenIddict Step-by-Step Guide](openiddict-step-by-step.md)
