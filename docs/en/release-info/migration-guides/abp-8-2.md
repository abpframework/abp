# ABP Version 8.2 Migration Guide

This document is a guide for upgrading ABP v8.x solutions to ABP v8.2. There are some changes in this version that may affect your applications, please read it carefully and apply the necessary changes to your application.

## Updated target frameworks to net8.0 for template projects

`TargetFrameworks` of the following template projects are upgraded to **net8.0**:

* `*.Application.Contracts`
* `*.Domain.Shared`
* `*.Domain`
* `*.MongoDB`
* `*.HttpApi.Client`
* `*.Host.Shared` (for module template)

Before this version, all of the projects above were targeting multiple frameworks (**netstandard2.0**, **netstandard2.1** and **net8.0**), with this version, we started to only target **net8.0** for these template projects. Note that, all other shared libraries still target multiple frameworks.

> This change should not affect your pre-existing solutions and you don't need to make any changes in your application. See the PR for more info: https://github.com/abpframework/abp/pull/19565

## Added default padding to `.tab-content` class for Basic Theme

In this version, default padding (padding-top: 1.5rem and padding-bottom: 1.5rem) has been added to the tab-content for the Basic Theme. See [#19475](https://github.com/abpframework/abp/pull/19475) for more information.

## Moved members page directory for Blogging Module

With this version on, ABP Framework allows you to use single blog mode, without needing to define a blog and a prefix. With this change, the following namespace changes were done:
* `Volo.Blogging.Pages.Blog` -> `Volo.Blogging.Pages.Blogs`
* `Volo.Blogging.Pages.Members` -> `Volo.Blogging.Pages.Blogs.Members` (members folder)

> If you haven't overridden the pages above, then you don't need to make any additional changes. See [#19418](https://github.com/abpframework/abp/pull/19418) for more information.
## Removed `FlagIcon` property from the `ILanguageInfo`

The `FlagIcon` property has been removed from the `ILanguageInfo` interface since we removed the flag icon library in the earlier versions from all of our themes and none of them using it now.

If the flag icon has been specified while defining the localization languages, then it should be removed:

```diff
        Configure<AbpLocalizationOptions>(options =>
        {
-           options.Languages.Add(new LanguageInfo("hi", "hi", "Hindi", "in"));
+          options.Languages.Add(new LanguageInfo("hi", "hi", "Hindi"));
             //...
        }
```

## Blazor Full-Stack Web UI

In this version, ABP Framework provides a new UI option called **Blazor Full-Stack WebApp**. We have already created an introduction/migration guide for you to check it: [Migrating to Blazor Web App](abp-8-2-blazor-web-app.md)

> Please read the documentation carefully if you are considering migrating your existing **Blazor** project to **Blazor WebApp**.

## Angular UI

In this version, the Angular UI has been updated to use the Angular version 17.3.0 and Nx version to 19.0.0. See the PR for more information: [#19915](https://github.com/abpframework/abp/pull/19915/)

## Session Management Infrastructure

The **Session Management** feature allows you to prevent concurrent login and manage user sessions.

In this version, a new entity called `IdentitySession` has been added to the framework and you should create a new migration and apply it to your database.

## Upgraded NuGet Dependencies

You can see the following list of NuGet libraries that have been upgraded with this release, if you are using one of these packages explicitly, you may consider upgrading them in your solution:

| Package                                                    | Old Version | New Version |
| ---------------------------------------------------------- | ----------- | ----------- |
| AutoMapper                                                 | 12.0.1      | 13.0.1      |
| Blazorise                                                  | 1.4.1       | 1.5.2       |
| Blazorise.Bootstrap5                                       | 1.4.1       | 1.5.2       |
| Blazorise.Icons.FontAwesome                                | 1.4.1       | 1.5.2       |
| Blazorise.Components                                       | 1.4.1       | 1.5.2       |
| Blazorise.DataGrid                                         | 1.4.1       | 1.5.2       |
| Blazorise.Snackbar                                         | 1.4.1       | 1.5.2       |
| Hangfire.AspNetCore                                        | 1.8.6       | 1.8.14      |
| Hangfire.SqlServer                                         | 1.8.6       | 1.8.14      |
| Microsoft.AspNetCore.Authentication.JwtBearer              | 8.0.0       | 8.0.4       |
| Microsoft.AspNetCore.Authentication.OpenIdConnect          | 8.0.0       | 8.0.4       |
| Microsoft.AspNetCore.Authorization                         | 8.0.0       | 8.0.4       |
| Microsoft.AspNetCore.Components                            | 8.0.0       | 8.0.4       |
| Microsoft.AspNetCore.Components.Authorization              | 8.0.0       | 8.0.4       |
| Microsoft.AspNetCore.Components.Web                        | 8.0.0       | 8.0.4       |
| Microsoft.AspNetCore.Components.WebAssembly                | 8.0.0       | 8.0.4       |
| Microsoft.AspNetCore.Components.WebAssembly.Server         | 8.0.0       | 8.0.4       |
| Microsoft.AspNetCore.Components.WebAssembly.Authentication | 8.0.0       | 8.0.4       |
| Microsoft.AspNetCore.Components.WebAssembly.DevServer      | 8.0.0       | 8.0.4       |
| Microsoft.AspNetCore.DataProtection.StackExchangeRedis     | 8.0.0       | 8.0.4       |
| Microsoft.AspNetCore.Mvc.NewtonsoftJson 		     | 8.0.0       | 8.0.4       |
| Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation          | 8.0.0       | 8.0.4       |
| Microsoft.AspNetCore.Mvc.Testing                           | 8.0.0       | 8.0.4       |
| Microsoft.AspNetCore.TestHost 		             | 8.0.0       | 8.0.4       |
| Microsoft.AspNetCore.WebUtilities 		             | 8.0.0       | 8.0.4       |
| Microsoft.Data.SqlClient                                   | 8.0.0       | 8.0.4       |
| Microsoft.EntityFrameworkCore                              | 8.0.0       | 8.0.4       |
| Microsoft.EntityFrameworkCore.Design                       | 8.0.0       | 8.0.4       |
| Microsoft.EntityFrameworkCore.InMemory                     | 8.0.0       | 8.0.4       |
| Microsoft.EntityFrameworkCore.Proxies                      | 8.0.0       | 8.0.4       |
| Microsoft.EntityFrameworkCore.Relational                   | 8.0.0       | 8.0.4       |
| Microsoft.EntityFrameworkCore.Sqlite                       | 8.0.0       | 8.0.4       |
| Microsoft.EntityFrameworkCore.SqlServer                    | 8.0.0       | 8.0.4       |
| Microsoft.EntityFrameworkCore.Tools                        | 8.0.0       | 8.0.4       |
| Microsoft.Extensions.DependencyInjection.Abstractions      | 8.0.0       | 8.0.1       |
| Microsoft.Extensions.FileProviders.Embedded                | 8.0.0       | 8.0.4       |
| Microsoft.Extensions.Logging.Abstractions                  | 8.0.0       | 8.0.1       |
| Microsoft.Extensions.Options                               | 8.0.0       | 8.0.2       |
| Microsoft.IdentityModel.Protocols.OpenIdConnect            | -           | 7.5.1       |
| Microsoft.IdentityModel.Tokens                             | -           | 7.5.1       |
| Microsoft.IdentityModel.JsonWebTokens                      | -           | 7.5.1       |
| System.IdentityModel.Tokens.Jwt                            | -           | 7.5.1       |
| OpenIddict.Abstractions                                    | 5.1.0       | 5.5.0       |
| OpenIddict.Core                                            | 5.1.0       | 5.5.0       |
| OpenIddict.Server.AspNetCore                               | 5.1.0       | 5.5.0       |
| OpenIddict.Validation.AspNetCore                           | 5.1.0       | 5.5.0       |
| OpenIddict.Validation.ServerIntegration                    | 5.1.0       | 5.5.0       |
| Oracle.EntityFrameworkCore                                 | 8.21.121    | 8.23.40     |
| Pomelo.EntityFrameworkCore.MySql                           | 8.0.0       | 8.0.2       |
| SixLabors.ImageSharp                                       | 3.0.2       | 3.1.4       |

