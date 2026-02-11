```json
//[doc-seo]
{
    "Description": "Learn how to develop mobile applications with the ABP Commercial platform using MAUI, integrating seamlessly with your ABP backend."
}
```

````json
//[doc-params]
{
  "Tiered": ["No", "Yes"]
}
````

# Getting Started with the MAUI

ABP Commercial platform provides a basic [MAUI](https://docs.microsoft.com/en-us/dotnet/maui/what-is-maui) template to develop mobile applications **integrated to your ABP based backends**.

## Run the Server Application

Run the backend application described in the [getting started document](../../../get-started/index.md).

## Run the Mobile Application

Open the `appsettings.json` in the `MAUI` project:

{{ if Tiered == "Yes" }}

* Make sure that `Authority` matches the running address of the `.AuthServer` project, `BaseUrl` matches the running address of the `.HttpApi.Host` project.

{{else}}

* Make sure that `Authority` and `BaseUrl` match the running address of the `.HttpApi.Host`, `.Web` or `.Blazor`(BlazorServer UI) projects.

{{ end }}

After ensuring the backend application is running and the `appsettings.json` is properly configured in the mobile application, you can proceed to run the mobile application. You can run the application either by using the `dotnet build` command (e.g. `dotnet build -t:Run -f net9.0-android` for Android or `dotnet build -t:Run -f net9.0-ios` for iOS) or by running it through Visual Studio or any other IDE that supports MAUI.

> For more information about running the mobile application, please refer to the [Microsoft's documentation](https://learn.microsoft.com/en-us/dotnet/maui/?view=net-maui-9.0).

You can examine the [Users Page](#users-page) or any other pre-defined page to see how to use CSharp Client Proxy to request backend API and consume the backend API in the same way in your application. Also, if you encounter any errors on specific platforms, you can refer to the following sections for each platform to find common issues and their solutions.

### Android

If you get the following error when connecting to the emulator or a physical phone, you need to set up port mapping.

```
Cannot connect to the backend on localhost. 
```

Open a command line terminal and run the `adb reverse` command to expose a port on your Android device to a port on your computer. For example:

`adb reverse tcp:44305 tcp:44305`

> Replace `44305` with the port number your backend application is running on.
>
> Run this command **after** the Android emulator has started.

> [!IMPORTANT]
> If your project uses a **tiered** or **microservice** architecture, ensure that both the **auth server** and all **remote service ports** are properly proxied using the `adb reverse` command. You can find all the required remote service ports and AuthServer configurations in your `YourProjectName.Maui/appsettings.json` file.

> [!NOTE]
> If you don't have a separate installation of **Android Debug Bridge** _(adb)_, you can open it from **Visual Studio** by following toolbar menu `Tools` > `Android` > `Android Adb Command Prompt`. Android emulator has to be running for this operation.


### iOS

The iOS simulator uses the host machine network. Therefore, applications running in the simulator can connect to web services running on your local machine via the machines IP address or via the localhost hostname. For example, given a local secure web service that exposes a GET operation via the /api/todoitems/ relative URI, an application running on the iOS simulator can consume the operation by sending a GET request to https://localhost:<port>/api/todoitems/.

> If the simulator is used from Windows with a remote connection, follow the [Microsoft's documentation](https://docs.microsoft.com/en-us/xamarin/cross-platform/deploy-test/connect-to-local-web-services#specify-the-local-machine-address) to setup a proper configuration.

#### Got could not find any available provisioning profiles for on ios error!

You need some extra steps, please check the [Microsoft document](https://learn.microsoft.com/en-us/xamarin/ios/get-started/installation/device-provisioning/)

#### Remote iOS Simulator for Windows

If you run the MAUI on a Mac agent, the remote iOS Simulator can't access the backend application running on Windows, you need to run the backend application on Mac or make the backend application internally.

## User Interface

The MAUI template consists of four pages: 

- **Homepage**: This is the welcome page of the application.
- **Users**: Management page for your application users. You can search, add, update, or delete users of your application.
- **Tenants**: Management page for your tenants. 
- **Settings**: Management page for your application settings. On this page, you can change **the current language**, **the profile picture**, **the current password**, or/and **the current theme**.

### Homepage

![Maui Home Page](../../../images/maui-home-page.png)

### Users Page

![Maui Users Page](../../../images/maui-users-page.png)

### Tenants Page

![Maui Tenants Page](../../../images/maui-tenants-page.png)

### Settings Page

![Maui Settings Page](../../../images/maui-settings-page.png)

### Advanced

#### Validation

In the context of the MVVM pattern (Model-View-ViewModel), a view model is responsible to perform data validation and signal any validation errors to the view so that the user can correct them. In MAUI, to perform validation you should define the view model properties as of type `ValidatableObject<T>` and specify related validations rules.

To specify validation rules and add validation rules to a property, you can refer to [Microsoft's Validation documentation](https://learn.microsoft.com/en-us/dotnet/architecture/maui/validation).

## Publishing
There is no custom step for publishing your app. You can follow the official documentation for each platform:
- [Android](https://learn.microsoft.com/en-us/dotnet/maui/android/deployment/?view=net-maui-8.0)
- [iOS](https://learn.microsoft.com/en-us/dotnet/maui/ios/deployment/?view=net-maui-8.0)
- [Mac Catalyst](https://learn.microsoft.com/en-us/dotnet/maui/mac-catalyst/deployment/?view=net-maui-8.0)
- [Windows](https://learn.microsoft.com/en-us/dotnet/maui/windows/deployment/overview?view=net-maui-8.0)

ABP template uses **Secure Storage** to store access & refresh tokens. So, make sure you have completed the following section for each platform to use it in production:
- [Secure Storage - Get Started](https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/storage/secure-storage?view=net-maui-8.0&tabs=android#get-started)
