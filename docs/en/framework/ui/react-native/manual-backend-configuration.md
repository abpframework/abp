```json
//[doc-seo]
{
  "Description": "Configure your ABP backend for React Native emulator and device testing using local IP addresses and HTTP when Cloudflare tunnels are unavailable."
}
```

```json
//[doc-params]
{
  "Architecture": ["Monolith", "Tiered", "Microservice"]
}
```

```json
//[doc-nav]
{
  "Previous": {
    "Name": "Running on Device",
    "Path": "framework/ui/react-native/running-on-device"
  }
}
```

# Manual Backend Configuration

> **When to use this guide:** We recommend this fallback when you cannot use the [Cloudflare tunnel](./running-on-device.md#cloudflare-tunnel-manual-cli) workflow — for example, on **Tiered** or **Microservice** templates (which do not include `tunnel.js`), when corporate firewalls block `cloudflared`, or when developing fully offline.

> **Prefer simpler options first:**
>
> - Browser testing → [Running on Web](./running-on-web.md) (no backend changes)
> - Pro, non-tiered Monolith on emulator/device → [Running on Device](./running-on-device.md) with `yarn tunnel:api` or the **MobileEmulator** profile

> React Native does not trust the auto-generated .NET HTTPS certificate. Use **HTTP** during this manual workflow.

{{ if Architecture == "Monolith" }}

To disable the HTTPS-only settings of OpenIddict, open the `MyProjectNameHttpApiHostModule` project and add the following code block to the `PreConfigureServices` method:

```csharp
#if DEBUG
    PreConfigure<OpenIddictServerBuilder>(options =>
    {
        options.UseAspNetCore()
            .DisableTransportSecurityRequirement();
    });
#endif
```

{{ else if Architecture == "Tiered" }}

To disable the HTTPS-only settings of OpenIddict, open the `MyProjectNameAuthServerModule` project and add the following code block to the `PreConfigureServices` method:

```csharp
#if DEBUG
    PreConfigure<OpenIddictServerBuilder>(options =>
    {
        options.UseAspNetCore()
            .DisableTransportSecurityRequirement();
    });
#endif
```

{{ end }}

> **Important:** Before running the backend application, make sure you have completed the [database migration](../../../get-started?UI=NG&DB=EF&Tiered=No#create-the-database) if you are starting with a fresh database.

A React Native application running on an Android emulator or a physical phone **cannot connect to the backend** on `localhost`. Bind the backend to `0.0.0.0` so it accepts requests from your local network, then update `Environment.ts` with your local IP address.

{{ if Architecture == "Monolith" }}

- Open the `appsettings.json` file in the `.DbMigrator` folder. Replace the `localhost` address in the `RootUrl` property with your local IP address. Then, run the database migrator.
- Open the `appsettings.Development.json` file in the `.HttpApi.Host` folder. Add this configuration to accept global requests for testing the React Native application in the development environment.
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

Update `react-native/Environment.ts` with your local IP address instead of `localhost`, then start Expo as described in [Running on Device](./running-on-device.md).

{{ else if Architecture == "Tiered" }}

- Open the `appsettings.json` file in the `.DbMigrator` folder. Replace the `localhost` address in the `RootUrl` property with your local IP address. Then, run the database migrator.
- Open the `appsettings.Development.json` file in the `.AuthServer` folder. Add this configuration to accept global requests for testing the React Native application in the development environment.
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
- Open the `appsettings.Development.json` file in the `.HttpApi.Host` folder. Add this configuration to accept global requests. Additionally, configure the authentication server as mentioned above.
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

Update `react-native/Environment.ts` with your local IP address, then start Expo as described in [Running on Device](./running-on-device.md).

{{ else if Architecture == "Microservice" }}

- Open the `appsettings.Development.json` file in the `.AuthServer` folder. Add this configuration to accept global requests for testing the React Native application in the development environment.
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
- Open the `appsettings.Development.json` file in the `.AdministrationService` folder. Add this configuration to accept global requests for testing the React Native application in the development environment. You should also provide the authentication server configuration. Additionally, apply the same process for all services you will use in the React Native application.
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
- Update the `appsettings.json` file in the `.IdentityService` folder. Replace the `localhost` configuration with your local IP address for the React Native application.
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
- Finally, update the mobile gateway configurations as follows:
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
            "Language": {
              "Address": "http://192.168.1.36:44310/"
            }
          }
        }
      }
    }
  }
  ```

Update `apps/mobile/react-native/Environment.ts` with your local IP address, then start Expo as described in [Running on Device](./running-on-device.md).

{{ end }}

Run the backend application(s) as described in the [getting started document](../../../get-started).
