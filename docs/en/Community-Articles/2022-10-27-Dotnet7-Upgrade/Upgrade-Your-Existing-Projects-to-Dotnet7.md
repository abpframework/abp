# Upgrade Your Existing  Projects to .NET7

A new .NET version has come. As open-source contributors, we are tracking the latest libraries and adopting them to our existing projects. In this sense, we completed our .NET 7 upgrade in our repositories for ABP Framework and ABP Commercial. In this article, I'll share the experiences we faced while upgrading to the new .NET version 👉  .NET 7.

When I wrote this article, the latest .NET version was `7.0.0-rc.2`. So some of the version statements I wrote below must be changed due to the stable version release.



**To see the latest and greatest stuff, let's see how to upgrade our existing projects to .NET 7!**



## Install .NET7 SDK

If you are on your development computer, then you need to install the .NET7 SDK `7.x.x`. For the production servers, you need to install the .NET 7 runtime. Download link for the .NET7 SDK and runtime is:

* https://dotnet.microsoft.com/en-us/download/dotnet/7.0



## Update Your *.csproj Files 

First, you need to update all your `*.csproj` files to support .NET7. Find and replace all your `<TargetFramework>*</TargetFramework>` in the `*.csproj` files to support .NET 7:

```xml
<TargetFramework>net7.0</TargetFramework>
```

We already did this in ABP Framework, see this commit as an example [github.com/abpframework/abp/templates/app-nolayers/aspnet-core/MyCompanyName.MyProjectName.Mvc.csproj](https://github.com/abpframework/abp/blob/dev/templates/app-nolayers/aspnet-core/MyCompanyName.MyProjectName.Mvc/MyCompanyName.MyProjectName.Mvc.csproj#L4).



### Microsoft Package Updates

You must be using Microsoft packages as well; then you need to update them to the latest .NET 7 version. 
At the time, I wrote this article, the latest version was `7.0.0-rc.2.22476.2`, so I'll update them to this version including minor version changes.

Here's the list of all package reference updates I did:

```xml
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="7.0.0-rc.2.*" />
<PackageReference Include="Microsoft.AspNetCore.Authentication.OpenIdConnect" Version="7.0.0-rc.2.*" />
<PackageReference Include="Microsoft.AspNetCore.Components" Version="7.0.0-rc.2.*" />
<PackageReference Include="Microsoft.AspNetCore.Components.Authorization" Version="7.0.0-rc.2.*" />
<PackageReference Include="Microsoft.AspNetCore.Components.Web" Version="7.0.0-rc.2.*" />
<PackageReference Include="Microsoft.AspNetCore.Components.WebAssembly" Version="7.0.0-rc.2.*" />
<PackageReference Include="Microsoft.AspNetCore.Components.WebAssembly.Authentication" Version="7.0.0-rc.2.*" />
<PackageReference Include="Microsoft.AspNetCore.Components.WebAssembly.DevServer" Version="7.0.0-rc.2.*" />
<PackageReference Include="Microsoft.AspNetCore.DataProtection.StackExchangeRedis" Version="7.0.0-rc.2.*" />
<PackageReference Include="Microsoft.AspNetCore.Mvc.NewtonsoftJson" Version="7.0.0-rc.2.*" />
<PackageReference Include="Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation" Version="7.0.0-rc.2.*" />
<PackageReference Include="Microsoft.AspNetCore.TestHost" Version="7.0.0-rc.2.*" />

<PackageReference Include="Microsoft.EntityFrameworkCore" Version="7.0.0-rc.2.*" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="7.0.0-rc.2.*">
<PackageReference Include="Microsoft.EntityFrameworkCore.Proxies" Version="7.0.0-rc.2.*" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="7.0.0-rc.2.*" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="7.0.0-rc.2.*" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="7.0.0-rc.2.*">
    
<PackageReference Include="Microsoft.Extensions.Caching.Memory" Version="7.0.0-rc.2.*" />
<PackageReference Include="Microsoft.Extensions.Caching.StackExchangeRedis" Version="7.0.0-rc.2.*" />
<PackageReference Include="Microsoft.Extensions.Configuration.UserSecrets" Version="7.0.0-rc.2.*" />
<PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="7.0.0-rc.2.*" />
<PackageReference Include="Microsoft.Extensions.DependencyInjection.Abstractions" Version="7.0.0-rc.2.*" />
<PackageReference Include="Microsoft.Extensions.FileProviders.Composite" Version="7.0.0-rc.2.*" />
<PackageReference Include="Microsoft.Extensions.FileProviders.Embedded" Version="7.0.0-rc.2.*" />
<PackageReference Include="Microsoft.Extensions.FileSystemGlobbing" Version="7.0.0-rc.2.*" />
<PackageReference Include="Microsoft.Extensions.Hosting" Version="7.0.0-rc.2.*" />
<PackageReference Include="Microsoft.Extensions.Http" Version="7.0.0-rc.2.*" />
<PackageReference Include="Microsoft.Extensions.Identity.Core" Version="7.0.0-rc.2.*" />
<PackageReference Include="Microsoft.Extensions.Localization" Version="7.0.0-rc.2.*" />
<PackageReference Include="Microsoft.Extensions.Logging.Console" Version="7.0.0-rc.2.*" />
    
<PackageReference Include="System.Text.Encoding.CodePages" Version="7.0.0-rc.2.*" />
<PackageReference Include="System.Text.Encodings.Web" Version="7.0.0-rc.2.*" />
<PackageReference Include="System.Text.Json" Version="7.0.0-rc.2.*" /> 

<PackageReference Include="Microsoft.VisualStudio.Web.CodeGeneration.Design" Version="7.0.0-rc.2.*" />
<PackageReference Include="System.Collections.Immutable" Version="7.0.0-rc.2.*" />
<PackageReference Include="Microsoft.Bcl.AsyncInterfaces" Version="7.0.0-rc.2.*" />
<PackageReference Include="Microsoft.Data.Sqlite" Version="7.0.0-rc.2.*" />
```



---



## Entity Framework Core Updates

If you use EF Core as your data access library, you should update your `dotnet-ef` CLI tool. Here's the terminal command to update it:

```bash
dotnet tool update dotnet-ef --global --prerelease
```

We already did the the EF Core package reference update in the *Microsoft Package Updates* section.



### Breaking Change: OrderBy

This release makes a breaking change in an EF Core query running behavior.  We faced this issue in some of our queries that were missing `OrderBy` statement. It throws an exception and does not run the query. Here's the explanation of a EF Core team member for this issue: [github.com/dotnet/efcore/issues/21202#issuecomment-913206415](https://github.com/dotnet/efcore/issues/21202#issuecomment-913206415).

The following exception is being thrown:

> InvalidOperationException: The query uses 'Skip' without specifying ordering and uses split query mode. This generates incorrect results. Either provide ordering or run query in single query mode using AsSingleQuery(). See https://go.microsoft.com/fwlink/?linkid=2196526 for more information

If you don't want to add `OrderBy` statement to solve the issue, you can also use  `AsSingleQuery()`.

![AsSingleQuery](https://raw.githubusercontent.com/abpframework/abp/dev/docs/en/Community-Articles/2022-10-27-Dotnet7-Upgrade/./as-single-query.jpg)



### EF Core - SQL Client Connection String Update

With this version, the behavior of the SQL connection has been changed. There is a keyword in the SQL connection string called `TrustServerCertificate`. This keyword indicates whether the channel will be encrypted while bypassing walking the certificate chain to validate  trust. 

> When `TrustServerCertificate` is set to `True`, the transport layer will use SSL to encrypt the channel and bypass walking the certificate chain to validate trust. If `TrustServerCertificate` is set to `true` and encryption is turned on, the encryption level specified on the server will be used even if `Encrypt` is set to `false`. The connection will fail otherwise.



After the .NET7 update, it just started to throw the following exception:

> A connection was successfully established with the server, but then an error occurred during the login process.



We fixed this problem by adding the `TrustServerCertificate=true` to your connection string. Here's an example connection string that supports, 

````sql
Server=localhost; Database=MyProjectName; Trusted_Connection=True; TrustServerCertificate=True
````

See our commit for this fix:

* [github.com/abpframework/abp/commit/96f17e67918eb87edd2baf876d4a7598281fe608](https://github.com/abpframework/abp/commit/96f17e67918eb87edd2baf876d4a7598281fe608)

Related docs:

* [learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-7.0/breaking-changes#encrypt-true](https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-7.0/breaking-changes#encrypt-true)
* [stackoverflow.com/questions/34430550/a-connection-was-successfully-established-with-the-server-but-then-an-error-occ](https://stackoverflow.com/questions/34430550/a-connection-was-successfully-established-with-the-server-but-then-an-error-occ)



---



## Blazor Update

Ensure that you have updated your Blazor project's csproj to support .NET7:

```xml
<TargetFramework>net7.0</TargetFramework>
```

### Install Blazor Workloads

#### .NET Web Assembly build tools

If you have a Blazor-WASM project, install the workloads by running the following in a command shell:

```bash
dotnet workload install wasm-tools
```

 **OR**  you can update your workloads by running the following command in your Blazor project directory:

```bash
dotnet workload restore
```



---



## .NET MAUI Update

Ensure that you have updated your Blazor project's csproj to support .NET7:

```xml
<TargetFrameworks>net7.0-android;net7.0-ios;net7.0-maccatalyst</TargetFrameworks>
<TargetFrameworks Condition="$([MSBuild]::IsOSPlatform('windows'))">$(TargetFrameworks);net7.0-windows10.0.19041.0</TargetFrameworks>
```



### Install MAUI Workloads

If you have .NET MAUI project, then you also need to update your `TargetFramework` as below:

If you have a .NET MAUI project, after installing the .NET 7 SDK, install the latest workloads with the following command:

```bash
dotnet workload install maui
```

 **OR**  run the following command in your existing .NET MAUI project directory

```bash
dotnet workload restore
```



Further information, check out https://github.com/dotnet/maui/wiki/.NET-7-and-.NET-MAUI

---



#### Dotnet Maui Check Tool

Alternatively, there's a 3rd party tool for .NET MAUI to install the required workloads. This tool installs the missing SDK packs. You can reach the tool's repository at [github.com/Redth/dotnet-maui-check](https://github.com/Redth/dotnet-maui-check).

Installation:

```bash
dotnet tool install -g Redth.Net.Maui.Check
```

Run:

```bash
maui-check
```



---



## Docker Image Update

If you are using Docker to automate the deployment of applications, you also need to update your images. We were using `aspnet:6.0.0-bullseye-slim` base and after the .NET 7 update, we started using `aspnet:7.0-bullseye-slim` in our Docker files. 

```
FROM mcr.microsoft.com/dotnet/aspnet:7.0-bullseye-slim AS base
```

For this update, you can check out the following commit as an example:

* [github.com/abpframework/abp/commit/2d07b9bd00152bef4658c48ff9b2cbee5788d308](https://github.com/abpframework/abp/commit/2d07b9bd00152bef4658c48ff9b2cbee5788d308)



## ABP Framework .NET 7 Update

In [ABP Framework repository](https://github.com/abpframework/abp), we updated all our dependencies from .NET 6 to .NET 7. 
Not all the changes are here, but you can check out the following PR of the .NET 7 update:

* [github.com/abpframework/abp/pull/13626/files](https://github.com/abpframework/abp/pull/13626/files)


...

Happy coding with .NET 7 🤗

...
