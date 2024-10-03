# OpenIddict MVC/Razor UI Migration Guide

## Web Project (Non-Tiered Solution)

- In **MyApplication.Web.csproj** replace **project references**:

  ```csharp
  <PackageReference Include="Volo.Abp.Account.Pro.Public.Web.IdentityServer" Version="6.0.*" />
  <PackageReference Include="Volo.Abp.IdentityServer.Web" Version="6.0.*" />
  ```

  with   

  ```csharp
  <PackageReference Include="Volo.Abp.Account.Pro.Public.Web.OpenIddict" Version="6.0.*" />
  <PackageReference Include="Volo.Abp.OpenIddict.Pro.Web" Version="6.0.*" />
  ```

- In **MyApplicationWebModule.cs** replace usings and **module dependencies**:

  ```csharp
  using Volo.Abp.IdentityServer.Web;
  ...
  typeof(AbpAccountPublicWebIdentityServerModule),
  typeof(AbpIdentityServerWebModule),
  ```

  with 

  ```csharp
  using Volo.Abp.OpenIddict.Pro.Web;
  ...
  typeof(AbpAccountPublicWebOpenIddictModule),
  typeof(AbpOpenIddictProWebModule),
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

  > Note: v6.0.0-rc.1 seems to be using `AddJwtBearer` for authorization. This is fixed in the next versions. If you are using v6.0.0-rc.1, it is safe to delete the jwt authentication and configure the authentication as shown above.

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

- In **MyApplicationWebModule.cs** `OnApplicationInitialization` method **remove IdentityServer and JwtToken midwares**:

  ```csharp
  app.UseIdentityServer();
  ```

## Web Project (Tiered Solution)

- In **MyApplication.Web.csproj** replace **project references**:

  ```csharp
  <PackageReference Include="Volo.Abp.IdentityServer.Web" Version="6.0.*" />
  ```
  
  with   
  
  ```csharp
  <PackageReference Include="Volo.Abp.OpenIddict.Pro.Web" Version="6.0.*" />
  ```
  
- In **MyApplicationWebModule.cs** replace usings and **module dependencies**:

  ```csharp
  using Volo.Abp.IdentityServer.Web;
  ...
  typeof(AbpIdentityServerWebModule),
  ```

  with 

  ```csharp
  using Volo.Abp.OpenIddict.Pro.Web;
  ...
  typeof(AbpOpenIddictProWebModule),
  ```

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
      options.GetClaimsFromUserInfoEndpoint = true;
  
      options.Scope.Add("roles"); // Replace "role" with "roles"
      options.Scope.Add("email");
      options.Scope.Add("phone");
      options.Scope.Add("MyApplication");
  });
  ```
  
  Replace role scope to **roles** and add the **UsePkce** option.

- In the **MyApplicationMenuContributor.cs** under *Menus* folder, **replace the using and menu name** under `ConfigureMainMenuAsync`:

  ```csharp
  using Volo.Abp.IdentityServer.Web.Navigation;
  ...
  //Administration->Identity Server
  administration.SetSubItemOrder(AbpIdentityServerMenuNames.GroupName, 2);
  ```

  with 

  ```csharp
  using Volo.Abp.OpenIddict.Pro.Web.Menus;
  ...
  //Administration->OpenIddict
  administration.SetSubItemOrder(OpenIddictProMenus.GroupName, 2);
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
  using Microsoft.AspNetCore.Authentication.JwtBearer;
  using IdentityServer4.Configuration;
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

- In **MyApplicationIdentityServerModule.cs** `OnApplicationInitialization` method **replace IdentityServer and JwtToken midware**:

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

* [OpenIddict Step-by-Step Guide](openIddict-step-by-step.md)
