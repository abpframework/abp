# OpenIddict Blazor-Server UI Migration Guide

## Blazor Project (Non-Tiered Solution)

- In the **MyApplication.Blazor.csproj** replace **project references**:

  ```csharp
  <PackageReference Include="Volo.Abp.Account.Pro.Public.Web.IdentityServer" Version="6.0.*" />
  <PackageReference Include="Volo.Abp.IdentityServer.Blazor.Server" Version="6.0.*" />
  ```

  with   

  ```csharp
  <PackageReference Include="Volo.Abp.Account.Pro.Public.Web.OpenIddict" Version="6.0.*" />
  <PackageReference Include="Volo.Abp.OpenIddict.Pro.Blazor.Server" Version="6.0.*" />    
  ```

- In the **MyApplicationBlazorModule.cs** replace usings and **module dependencies**:

  ```csharp
  using Volo.Abp.AspNetCore.Authentication.JwtBearer;
  using Volo.Abp.IdentityServer.Blazor.Server;
  ...
  typeof(AbpAccountPublicWebIdentityServerModule),
  typeof(AbpIdentityServerBlazorServerModule),
  ```

  with 

  ```csharp
  using OpenIddict.Validation.AspNetCore;
  using Volo.Abp.OpenIddict.Pro.Blazor.Server;
  ...
  typeof(AbpAccountPublicWebOpenIddictModule),
  typeof(AbpOpenIddictProBlazorServerModule),
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

  ```
  app.UseAbpOpenIddictValidation();
  ```

- In the **MyApplicationMenuContributor.cs** under *Menus* folder, **replace the using and menu name** under `ConfigureMainMenuAsync`:

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

- In **MyApplicationIdentityServerModule.cs** replace usings and **module dependencies**:

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
