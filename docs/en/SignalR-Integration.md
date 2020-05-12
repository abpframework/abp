## SignalR Integration

> It is already possible to follow [the standard Microsoft tutorial](https://docs.microsoft.com/en-us/aspnet/core/tutorials/signalr) to add [SignalR](https://docs.microsoft.com/en-us/aspnet/core/signalr/introduction) to your application. However, ABP provides a SignalR integration packages that simplify the integration and usage.

## Installation

### Server Side

It is suggested to use the [ABP CLI](CLI.md) to install this package.

#### Using the ABP CLI

Open a command line window in the folder of your project (.csproj file) and type the following command:

```bash
abp add-package Volo.Abp.AspNetCore.SignalR
```

> You typically want to add this package to the web or API layer of your application, depending on your architecture.

#### Manual Installation

If you want to manually install;

1. Add the [Volo.Abp.AspNetCore.SignalR](https://www.nuget.org/packages/Volo.Abp.AspNetCore.SignalR) NuGet package to your project:

   ```
   Install-Package Volo.Abp.BackgroundJobs.HangFire
   ```

   Or use the Visual Studio NuGet package management UI to install it.

2. Add the `AbpAspNetCoreSignalRModule` to the dependency list of your module:

```csharp
[DependsOn(
    //...other dependencies
    typeof(AbpAspNetCoreSignalRModule) //Add the new module dependency
    )]
public class YourModule : AbpModule
{
}
```

> You don't need to use the `services.AddSignalR()` and the `app.UseEndpoints(...)`, it's done by the `AbpAspNetCoreSignalRModule`.

### Client Side

Client side installation depends on your UI framework.

#### ASP.NET Core MVC / Razor Pages UI

Run the following command in the root folder of your web project:

````bash
yarn add @abp/signalr
````

> This requires to [install yarn](https://yarnpkg.com/) if you haven't install before.

This will add the `@abp/signalr` to the dependencies in the `package.json` of your project:

````json
{
  ...
  "dependencies": {
    ...
    "@abp/signalr": "~2.7.0"
  }
}
````

Run the `gulp` in the root folder of your web project:

````bash
gulp
````

This will copy the SignalR JavaScript files into your project:

![signal-js-file](D:\Github\abp\docs\en\images\signal-js-file.png)

Finally, add the following code to your page/view to include the `signalr.js` file 

````xml
@section scripts {
    <abp-script type="typeof(SignalRBrowserScriptContributor)" />
}
````

It requires to add `@using Volo.Abp.AspNetCore.Mvc.UI.Packages.SignalR` to your page/view.

> You could add the `signalr.js` file in a standard way. But using the `SignalRBrowserScriptContributor` has additional benefits. See the [Client Side Package Management](UI/AspNetCore/Client-Side-Package-Management.md) and [Bundling & Minification](UI/AspNetCore/Bundling-Minification.md) documents for details.

That's all. you can use the [SignalR JavaScript API](https://docs.microsoft.com/en-us/aspnet/core/signalr/javascript-client) in your page.

#### Other UI Frameworks / Clients

Please refer to [Microsoft's documentation](https://docs.microsoft.com/en-us/aspnet/core/signalr/introduction) for other type of clients.

## The ABP Framework Integration

This section covers the additional benefits when you use the ABP Framework integration packages.

### Hub Route & Mapping

ABP automatically registers Hubs to the [dependency injection](Dependency-Injection.md) (as transient) and maps the hub endpoint. So, you don't have to use the ` app.UseEndpoints(...)` to map your hubs. Hub route (URL) is determined conventionally based on your hub name.

Example:

````csharp
public class MessagingHub : Hub
{
    //...
}
````

The hub route will be `/signalr-hubs/messasing` for the `MessasingHub`:

* Adding a standard `/signalr-hubs/` prefix
* Continue with the **camel case** hub name, without the `Hub` postfix.

If you want to specify the route, you can use the `HubRoute` attribute:

````csharp
[HubRoute("/my-messasing-hub")]
public class MessagingHub : Hub
{
    //...
}
````

### AbpHub Base Class

Instead of the standard `Hub` class, you can inherit from the `AbpHub` which has useful base properties, like `CurrentUser`.

Example:

````csharp
public class MessagingHub : AbpHub
{
    public async Task SendMessage(string targetUserName, string message)
    {
        var currentUserName = CurrentUser.UserName; //Access to the current user info
        var txt = L["MyText"]; //Localization
    }
}
````



## Example Application

See the [SignalR Integration Demo](https://github.com/abpframework/abp-samples/tree/master/SignalRDemo) as a sample application. It has a simple Chat page to send messaged between (authenticated) users.