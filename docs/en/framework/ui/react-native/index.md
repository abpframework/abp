```json
//[doc-seo]
{
    "Description": "Learn how to set up your development environment for React Native with ABP Framework, enabling seamless mobile app integration!"
}
```

````json
//[doc-params]
{
  "Architecture": ["Monolith", "Tiered", "Microservice"]
}
````

# Getting Started with the React Native

> React Native mobile option is *available for* ***Team*** *or higher licenses*

ABP platform provide basic [React Native](https://reactnative.dev/) startup template to develop mobile applications **integrated to your ABP based backends**.

![React Native gif](../../../images/react-native-introduction.gif)

## How to Prepare Development Environment

Please follow the steps below to prepare your development environment for React Native.

1. **Install Node.js:** Please visit [Node.js downloads page](https://nodejs.org/en/download/) and download proper Node.js v20.11+ installer for your OS. An alternative is to install [NVM](https://github.com/nvm-sh/nvm) and use it to have multiple versions of Node.js in your operating system.
2. **[Optional] Install Yarn:** You may install Yarn v1 (not v2) following the instructions on [the installation page](https://classic.yarnpkg.com/en/docs/install). Yarn v1 delivers an arguably better developer experience compared to npm v6 and below. You may skip this step and work with npm, which is built-in in Node.js, instead.
3. **[Optional] Install VS Code:** [VS Code](https://code.visualstudio.com/) is a free, open-source IDE which works seamlessly with TypeScript. Although you can use any IDE including Visual Studio or Rider, VS Code will most likely deliver the best developer experience when it comes to React Native projects.
4. **Install an Emulator/Simulator:** React Native applications need an Android emulator or an iOS simulator to run on your OS. If you do not have Android Studio installed and configured on your system, we recommend [setting up android emulator without android studio](setting-up-android-emulator.md).

If you prefer the other way, you can check the [Android Studio Emulator](https://docs.expo.dev/workflow/android-studio-emulator/) or [iOS Simulator](https://docs.expo.dev/workflow/ios-simulator/) on expo.io documentation to learn how to set up an emulator.

## How to Start a New React Native Project

You have multiple options to initiate a new React Native project that works with ABP:

### 1. Using ABP Studio

ABP Studio application is the most convenient and flexible way to initiate a React Native application based on ABP framework. You can follow the [tool documentation](../../../studio) and select the option below:

![React Native option](../../../images/react-native-option.png)

### 2. Using ABP CLI

ABP CLI is another way of creating an ABP solution with a React Native application. Simply [install the ABP CLI](../../../cli) and run the following command in your terminal:

```shell
abp new MyCompanyName.MyProjectName -csf -u <angular or mvc> -m react-native
```

> To see further options in the CLI, please visit the [CLI manual](../../../cli).

This command will prepare a solution with an **Angular** or an **MVC** (depends on your choice), a **.NET Core**, and a **React Native** project in it.

## How to Configure & Run the Backend

> React Native application does not trust the auto-generated .NET HTTPS certificate. You should use **HTTP** during the development.

A React Native application running on an Android emulator or a physical phone **can not connect to the backend** on `localhost`. To fix this problem, it is necessary to run the backend application using the `Kestrel` configuration.

{{ if Architecture == "Monolith" }}

![React Native monolith host project configuration](../../../images/react-native-monolith-be-config.png)

- Open the `appsettings.json` file in the `.DbMigrator` folder. Replace the `localhost` address on the `RootUrl` property with your local IP address. Then, run the database migrator.
- Open the `appsettings.Development.json` file in the `.HttpApi.Host` folder. Add this configuration to accept global requests just to test the react native application on the development environment.

  ```json
  {
    "Kestrel": {
      "Endpoints": {
        "Http": {
          "Url": "http://0.0.0.0:44323" //replace with your host port
        }
      }
    }
  }
  ```

{{ else if Architecture == "Tiered" }}

![React Native tiered project configuration](../../../images/react-native-tiered-be-config.png)

- Open the `appsettings.json` file in the `.DbMigrator` folder. Replace the `localhost` address on the `RootUrl` property with your local IP address. Then, run the database migrator.
- Open the `appsettings.Development.json` file in the `.AuthServer` folder. Add this configuration to accept global requests just to test the react native application on the development environment.

  ```json
  {
    "Kestrel": {
      "Endpoints": {
        "Http": {
          "Url": "http://0.0.0.0:44337"
        }
      }
    }
  }
  ```

- Open the `appsettings.Development.json` file in the `.HttpApi.Host` folder. Add this configuration to accept global requests again. In addition, you will need to introduce the authentication server as mentioned above.

  ```json
  {
    "Kestrel": {
      "Endpoints": {
        "Http": {
          "Url": "http://0.0.0.0:44389" //replace with your host port
        }
      }
    },
    "AuthServer": {
      "Authority": "http://192.168.1.37:44337/", //replace with your IP and authentication port
      "MetaAddress": "http://192.168.1.37:44337/",
      "RequireHttpsMetadata": false,
      "Audience": "MyTieredProject" //replace with your application name
    }
  }
  ```

{{ else if Architecture == "Microservice" }}

![React Native microservice project configuration](../../../images/react-native-microservice-be-config.png)

- Open the `appsettings.Development.json` file in the `.AuthServer` folder. Add this configuration to accept global requests just to test the react native application on the development environment.

  ```json
  {
    "App": {
      "EnablePII": true
    },
    "Kestrel": {
      "Endpoints": {
        "Http": {
          "Url": "http://0.0.0.0:44319"
        }
      }
    }
  }
  ```

- Open the `appsettings.Development.json` file in the `.AdministrationService` folder. Add this configuration to accept global requests just to test the react native application on the development environment. You should also provide the authentication server configuration. In addition, you need to apply the same process for all the services you would use in the react native application.

  ```json
  {
    "App": {
      "EnablePII": true
    },
    "Kestrel": {
      "Endpoints": {
        "Http": {
          "Url": "http://0.0.0.0:44357"
        }
      }
    },
    "AuthServer": {
      "Authority": "http://192.168.1.36:44319/",
      "MetaAddress": "http://192.168.1.36:44319/",
      "RequireHttpsMetadata": false,
      "Audience": "AdministrationService"
    }
  }
  ```

- Update the `appsettings.json` file in the `.IdentityService` folder. Replace the `localhost` configuration with your local IP address for the react native application

  ```json
  {
    //...
    "OpenIddict": {
      "Applications": {
        //...
        "ReactNative": {
          "RootUrl": "exp://192.168.1.36:19000"
        },
        "MobileGateway": {
          "RootUrl": "http://192.168.1.36:44347/"
        }
        //...
      }
      //...
    }
  }
  ```

- Lastly, update the mobile gateway configurations as following:

  ```json
  //gateways/mobile/MyMicroserviceProject.MobileGateway/Properties/launchSettings.json
  {
    "iisSettings": {
      "windowsAuthentication": false,
      "anonymousAuthentication": true,
      "iisExpress": {
        "applicationUrl": "http://192.168.1.36:44347" //update with your IP address
      }
    },
    "profiles": {
      //...
      "MyMicroserviceProject.MobileGateway": {
        "commandName": "Project",
        "dotnetRunMessages": "true",
        "launchBrowser": true,
        "applicationUrl": "http://192.168.1.36:44347",
        "environmentVariables": {
          "ASPNETCORE_ENVIRONMENT": "Development"
        }
      }
    }
  }
  ```

  ```json
  //gateways/mobile/MyMicroserviceProject.MobileGateway/appsettings.json
  {
    //Update clusters with your IP address
    //...
    "ReverseProxy": {
      //...
      "Clusters": {
        "AuthServer": {
          "Destinations": {
            "AuthServer": {
              "Address": "http://192.168.1.36:44319/"
            }
          }
        },
        "Administration": {
          "Destinations": {
            "Administration": {
              "Address": "http://192.168.1.36:44357/"
            }
          }
        },
        "Saas": {
          "Destinations": {
            "Saas": {
              "Address": "http://192.168.1.36:44330/"
            }
          }
        },
        "Identity": {
          "Destinations": {
            "Identity": {
              "Address": "http://192.168.1.36:44397/"
            }
          }
        },
        "Language": {
          "Destinations": {
            "Identity": {
              "Address": "http://192.168.1.36:44310/"
            }
          }
        }
      }
    }
  }
  ```

  {{ end }}

Run the backend application as described in the [getting started document](../../../get-started).

> You should turn off the "Https Restriction" if you're using OpenIddict as a central identity management solution. Because the IOS Simulator doesn't support self-signed certificates and OpenIddict is set to only work with HTTPS by default.

## How to disable the Https-only settings of OpenIddict

Open the {{ if Architecture == "Monolith" }}`MyProjectNameHttpApiHostModule`{{ else if Architecture == "Tiered" }}`MyProjectNameAuthServerModule`{{ end }} project and copy-paste the below code-block to the `PreConfigureServices` method:

```csharp
#if DEBUG
    PreConfigure<OpenIddictServerBuilder>(options =>
    {
        options.UseAspNetCore()
            .DisableTransportSecurityRequirement();
    });
#endif
```

## How to Configure & Run the React Native Application

1. Make sure the [database migration is complete](../../../get-started?UI=NG&DB=EF&Tiered=No#create-the-database) and the [API is up and running](../../../get-started?UI=NG&DB=EF&Tiered=No#run-the-application).
2. Open `react-native` folder and run `yarn` or `npm install` if you have not already.
3. Open the `Environment.ts` in the `react-native` folder and replace the `localhost` address on the `apiUrl` and `issuer` properties with your local IP address as shown below:

{{ if Architecture == "Monolith" }}

![react native monolith environment local IP](../../../images/react-native-monolith-environment-local-ip.png)

{{ else if Architecture == "Tiered" }}

![react native tiered environment local IP](../../../images/react-native-tiered-environment-local-ip.png)

> Make sure that `issuer` matches the running address of the `.AuthServer` project, `apiUrl` matches the running address of the `.HttpApi.Host` or `.Web` project.

{{ else }}

![react native microservice environment local IP](../../../images/react-native-microservice-environment-local-ip.png)

> Make sure that `issuer` matches the running address of the `.AuthServer` project, `apiUrl` matches the running address of the `.AuthServer` project.

{{ end }}

1. Run `yarn start` or `npm start`. Wait for the Expo CLI to print the opitons.

> The React Native application was generated with [Expo](https://expo.io/). Expo is a set of tools built around React Native to help you quickly start an app and, while it has many features.

![expo-cli-options](../../../images/rn-options.png)

In the above image, you can start the application with an Android emulator, an iOS simulator or a physical phone by scanning the QR code with the [Expo Client](https://expo.io/tools#client) or choosing the option.

### Expo

![React Native login screen on iPhone 16](../../../images/rn-login-iphone.png)

### Android Studio

1. Start the emulator in **Android Studio** before running the `yarn start` or `npm start` command.
2. Press **a** to open in Android Studio.

![React Native login screen on Android Device](../../../images/rn-login-android-studio.png)

Enter **admin** as the username and **1q2w3E\*** as the password to login to the application.

The application is up and running. You can continue to develop your application based on this startup template.

## See Also

- [React Native project structure](../../../solution-templates/application-module#react-native)
