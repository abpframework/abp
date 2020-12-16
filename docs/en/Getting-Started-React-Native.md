# Getting Started with the React Native

````json
//[doc-params]
{
    "Tiered": ["No", "Yes"]
}
````

ABP platform provide basic [React Native](https://reactnative.dev/) startup template to develop mobile applications **integrated to your ABP based backends**.

![React Native gif](./images/react-native-introduction.gif)

## How to Prepare Development Environment

Please follow the steps below to prepare your development environment for React Native.

1. **Install Node.js:** Please visit [Node.js downloads page](https://nodejs.org/en/download/) and download proper Node.js v12 or v14 installer for your OS. An alternative is to install [NVM](https://github.com/nvm-sh/nvm) and use it to have multiple versions of Node.js in your operating system.
2. **[Optional] Install Yarn:** You may install Yarn v1 (not v2) following the instructions on [the installation page](https://classic.yarnpkg.com/en/docs/install). Yarn v1 delivers an arguably better developer experience compared to npm v6 and below. You may skip this step and work with npm, which is built-in in Node.js, instead.
3. **[Optional] Install VS Code:** [VS Code](https://code.visualstudio.com/) is a free, open-source IDE which works seamlessly with TypeScript. Although you can use any IDE including Visual Studio or Rider, VS Code will most likely deliver the best developer experience when it comes to React Native projects.
4. **Install an Emulator:** React Native applications need an Android emulator or an iOS simulator to run on your OS. See the [Android Studio Emulator](https://docs.expo.io/workflow/android-simulator/) or [iOS Simulator](https://docs.expo.io/workflow/ios-simulator/) on expo.io documentation to learn how to set up an emulator.


## How to Start a New React Native Project

You have multiple options to initiate a new React Native project that works with ABP:

### 1. Using ABP CLI

ABP CLI is probably the most convenient and flexible way to initiate an ABP solution with a React Native application. Simply [install the ABP CLI](CLI.md) and run the following command in your terminal:

```shell
abp new MyCompanyName.MyProjectName -csf -u <angular or mvc> -m react-native
```

> To see further options in the CLI, please visit the [CLI manual](CLI.md).

This command will prepare a solution with an **Angular** or an **MVC** (depends on your choice), a **.NET Core**, and a **React Native** project in it.

### 2. Direct Download

You may [download a solution scaffold directly on ABP.io](https://abp.io/get-started) if you are more comfortable with GUI or simply want to try ABP without installing the CLI.

Please do the following:

1. Click on the "DIRECT DOWNLOAD" tab.
2. Fill out the short form about your project.
3. Click on the "Create now" button.

...and a customized download will start in a few seconds.

## How to Configure & Run the Backend

> React Native application does not trust the auto-generated .NET HTTPS certificate. You should use **HTTP** during the development.

A React Native application running on an Android emulator or a physical phone **can not connect to the backend** on `localhost`. To fix this problem, it is necessary to run the backend application on your **local IP address**.

{{ if Tiered == "No"}}
![React Native host project local IP entry](images/rn-host-local-ip.png)

* Open the `appsettings.json` in the `.HttpApi.Host` folder. Replace the `localhost` address on the `SelfUrl` and `Authority` properties with your local IP address.
* Open the `launchSettings.json` in the `.HttpApi.Host/Properties` folder. Replace the `localhost` address on the `applicationUrl` properties with your local IP address.

{{ else if Tiered == "Yes" }}

![React Native tiered project local IP entry](images/rn-tiered-local-ip.png)

* Open the `appsettings.json` in the `.IdentityServer` folder. Replace the `localhost` address on the `SelfUrl` property with your local IP address.
* Open the `launchSettings.json` in the `.IdentityServer/Properties` folder. Replace the `localhost` address on the `applicationUrl` properties with your local IP address.
* Open the `appsettings.json` in the `.HttpApi.Host` folder. Replace the `localhost` address on the `Authority` property with your local IP address.
* Open the `launchSettings.json` in the `.HttpApi.Host/Properties` folder. Replace the `localhost` address on the `applicationUrl` properties with your local IP address.

{{ end }}

Run the backend application as described in the [getting started document](Getting-Started.md).


## How to Configure & Run the React Native Application

1. Make sure the [database migration is complete](./Getting-Started?UI=NG&DB=EF&Tiered=No#create-the-database) and the [API is up and running](./Getting-Started?UI=NG&DB=EF&Tiered=No#run-the-application).
2. Open `react-native` folder and run `yarn` or `npm install` if you have not already.
3. Open the `Environment.js` in the `react-native` folder and replace the `localhost` address on the `apiUrl` and `issuer` properties with your local IP address as shown below:

![react native environment local IP](images/rn-environment-local-ip.png)

{{ if Tiered == "Yes" }}

> Make sure that `issuer` matches the running address of the `.IdentityServer` project, `apiUrl` matches the running address of the `.HttpApi.Host` or `.Web` project.

{{else}}

> Make sure that `issuer` and `apiUrl` matches the running address of the `.HttpApi.Host` or `.Web` project.

{{ end }}

4. Run `yarn start` or `npm start`. Wait Expo CLI to start. Expo CLI opens the management interface on the `http://localhost:19002/` address.

> The React Native application was generated with [Expo](https://expo.io/). Expo is a set of tools built around React Native to help you quickly start an app and, while it has many features.

![expo-interface](images/rn-expo-interface.png)

In the above management interface, you can start the application with an Android emulator, an iOS simulator or a physical phone by the scan the QR code with the [Expo Client](https://expo.io/tools#client).

![React Native login screen on iPhone 11](images/rn-login-iphone.png)

Enter **admin** as the username and **1q2w3E*** as the password to login to the application.

The application is up and running. You can continue to develop your application based on this startup template.


## See Also

* [React Native project structure](./Startup-Templates/Application#react-native)
