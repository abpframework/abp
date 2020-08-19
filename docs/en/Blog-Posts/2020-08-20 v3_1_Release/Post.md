# ABP Framework v3.1 RC.1 Has Been Released

Today, we are releasing the **ABP Framework version 3.1 Release Candidate 1** (RC.1). The development cycle for this version was ~7 weeks. It was the longest development cycle for a feature version release ever. There were two main reasons of this long development cycle;

* We've switched to **4-weeks** release cycle (was discussed in [this issue](https://github.com/abpframework/abp/issues/4692)).
* We've [re-written](https://github.com/abpframework/abp/issues/4881) the Angular service proxy generation system using the Angular schematics to make it more stable. There were some problems with the previous implementation.

This long development cycle brings a lot of new features, improvements and bug fixes. I will highlight the fundamental features and changes in this blog post.

## About the Preview/Stable Version Cycle

As mentioned above, it is planned to release a new stable feature version (like 3.1, 3.2, 3.3...) in every 4-weeks.

In addition, we are starting to deploy **Release Candidate (RC)** versions 2-weeks before the stable versions for every feature releases. You can think these releases as "**preview**" version.

Today, we've released `3.1.0-rc.1` as the first RC/Preview version. We may release more RC versions if it is needed until the stable version.

The stable `3.1.0` version will be released on September 3, 2020. Next RC version, `3.2.0-rc.1` was planned for September 17, 2020 (2 weeks after the stable 3.1 version and 2 weeks before the stable 3.2 version).

> We **won't add new features** to a version after publishing the RC/Preview version. We only will make **bug fixes** until the stable version. The new features being developed in this period will be available in the next version.

### About the Nightly Builds

Don't confuse RC/Preview versions and nightly builds. When we say RC or preview, we are mentioning the preview system explained above.

We will continue to publish **nightly builds** for all the [ABP Framework packages](https://abp.io/packages). You can refer to [this document](https://docs.abp.io/en/abp/latest/Nightly-Builds) to learn how to use the nightly packages.

## Get Started with the RC Versions

Please try the preview versions and provide feedback to us to release more stable versions. Please open an  issue on the [GitHub repository](https://github.com/abpframework/abp/issues/new) if you find a bug.

### Update the ABP CLI to the 3.1.0-rc.1

***TODO***

### New Solutions

The [ABP.IO](https://abp.io/) platform and the [ABP CLI](https://docs.abp.io/en/abp/latest/CLI) are compatible with the RC system. You can select the "preview" option on the [download page](https://abp.io/get-started) or use the "**--preview**" parameter with the ABP CLI [new](https://docs.abp.io/en/abp/latest/CLI?_ga=2.106435654.411298747.1597771169-1910388957.1594128976#new) command:

````bash
abp new Acme.BookStore --preview
````

This command will create a new project with the latest RC/Preview version. Whenever the stable version is released, you can switch to the stable version for your solution using the `abp switch-to-stable` command in the root folder of your solution.

### Existing Solutions

If you already have a solution and want to use/test the latest RC/Preview version, use the `abp switch-to-preview` command in the root folder of your solution. You can return back to the latest stable using the `abp switch-to-stable ` command later.

> Note that the `abp switch-to-preview` command was being used to switch to nightly builds before the v3.1. Now, you should use the `abp switch-to-nightly` for [nightly builds](https://docs.abp.io/en/abp/latest/Nightly-Builds).

## What's New with the ABP Framework 3.1

### Angular Service Proxies

ABP provides a system to generate Angular service proxies (with TypeScript) to consume the HTTP APIs of your application. Service proxy generation system **has been completely re-written** with the ABP Framework 3.1. The main goal was to build more stable and feature rich system that is better aligned with other ABP Framework features (like [modularity](https://docs.abp.io/en/abp/latest/Module-Development-Basics)).

[See the documentation](https://docs.abp.io/en/abp/latest/UI/Angular/Service-Proxies) to learn more about the service proxy generation for Angular applications.

### Authorization Code Flow for the Angular UI

We were using the **resource owner password authentication** flow for the Angular UI login page. We've implemented **Authorization Code Flow** for the Angular account module and made it **default for new projects**. With this change, the Angular application now redirects to the login page of the MVC UI which was implemented using the Identity Server 4. We also removed the client secret from the Angular side with this change.

Old behavior remains exist. If you want to switch to the new flow (which is recommended), follow the steps below:

1) Add `authorization_code` to the `IdentityServerClientGrantTypes` table in the database, for the client used by the Angular UI (the `ClientId` is `YourProjectName_App` by default, in the `IdentityServerClients` table).

2) Add `http://localhost:4200` to `IdentityServerClientRedirectUris` and `IdentityServerClientPostLogoutRedirectUris` tables for the same client.

3) Set `RequireClientSecret` to `false` in the `IdentityServerClients` table for the same client.

> [ABP Commercial](https://commercial.abp.io/) users can make these changes on the [Identity Server Management UI](https://commercial.abp.io/modules/Volo.Identityserver.Ui).

4) Change the `oAuthConfig` section in the `src/environments/environment.ts` file of the Angular application.

You can take [this new configuration](https://github.com/abpframework/abp/blob/dev/templates/app/angular/src/environments/environment.ts) as a reference. Main changes are;

* Added `responseType` as `code`.
* Added `redirectUri`
* Added `offline_access` to the `scope`.
* Removed `oidc: false` option.
* Removed the client secret option.

### Global Feature System

The new "Global Features" system allows to **enable/disable features of an application or a module** in a central point. It is especially useful if you want to use a module but don't want to bring all its features into your application. If the module was so designed, you can enable only the features you need. 

When you disable a feature;

* The **database tables** related to that feature should not be included in the database.
* The **HTTP APIs** related to that feature should not be exposed. They returns 404 if they are directly requested.

So, the goal is that; when you disable a feature, it should behave like that feature doesn't exists in your system at all.

There is **no way to enable/disable a global feature on runtime**. You should decide it in the development time (remember, even database tables are not being created for disabled global features, so you can't enable it on runtime).

> "Global Features" system is different than [SaaS/multi-tenancy features](https://docs.abp.io/en/abp/latest/Features), where you can enable/disable features for your tenants on runtime.

Assume that you are using the [CMS Kit module](https://github.com/abpframework/abp/tree/dev/modules/cms-kit) (this module is in a very early stage) where you only want to enable the comment feature:

````csharp
GlobalFeatureManager.Instance.Modules.CmsKit().Comments.Enable();
````

You can check if a feature was enabled:

```csharp
GlobalFeatureManager.Instance.IsEnabled<CommentsFeature>();
```

Or you can add `[RequiresGlobalFeature(...)]` attribute to a controller/page to disable it if the related feature was disabled:

```csharp
//...
[RequiresGlobalFeature(typeof(CommentsFeature))]
public class CommentController :  AbpController
{
    //...
}
```

See the issue [#5061](https://github.com/abpframework/abp/issues/5061) until this is fully documented.

### New Account Module Features

TODO

### New Identity Module Features

TODO

## What's New with the ABP Commercial v3.1

### New Account Module Features

TODO

### New Identity Module Features

TODO

### Others

TODO