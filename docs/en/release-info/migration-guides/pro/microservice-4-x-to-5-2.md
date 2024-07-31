# Microservice Template Version 4.x to 5.2 Migration Guide

This document is a guide for upgrading Microservice template version 4.4 to version 5.2. Please read them all since there are some important breaking changes. 

 It is strongly suggested to follow [General Startup Template Changes Guide](https://docs.abp.io/en/abp/5.2/Migration-Guides/Upgrading-Startup-Template) and generate an up-to-date microservice template for comparison. Compare your code-base for each application, gateways, microservices, and the shared projects that you are using.

## .NET 6.0

ABP 5.0 runs on .NET 6.0. So, please upgrade your microservice template solution to .NET 6.0 if you want to update your solution to ABP 5.x. You can see [Microsoft's migration guide](https://docs.microsoft.com/en-us/aspnet/core/migration/50-to-60).

## General Enhancements

- Added Docker support. Each application, gateway, and microservices have 2 different docker files. **Dockerfile** for building CI/CD friendly, multi-stage, cached image building in containers. **Dockerfile.local** for building fast, SDK required image building. 
- Added Powershell scripts under the *build* folder to build docker images easily. Use either **build-images.ps1** to build CI/CD friendly multi-stage docker images or **build-images-locally.ps1** to build docker images locally.
- Added **Kubernetes** and **Helm** support. Under *etc/k8s* folder, now you can find helm charts for your template. While you can deploy your whole application as a single helm chart with **YourProjectName**, you can also deploy each application individually. Each helm chart has **values.yaml** file needs to be filled if you want to deploy individually. YourProjectName helm chart **values.yaml** overrides the individual values if you deploy as a single unit. We have also added **deploy-staging.ps1** Powershell script to deploy YourProjectName with a namespace that you can use and modify.
- **Replaced dynamic proxying** with the static proxying in gateways. The main reason for this decision is to free gateways from microservice dependencies. When the gateways are no longer dependent on any microservice layer, you can switch gateway from ocelot to any other gateway you desire. 
- **Removed Internal Gateway**. We decided that adding an extra gateway between microservices is unnecessary and microservices can communicate with each other without a gateway redirection. This will also reduce the complication of inter-microservice communication and deployments.

## AuthServer

AuthServer is now hosting Account Http.Api module. Since account-related management and configurations are done in AuthServer, hosting responsibility is now moved to AuthServer with swagger support.

- AuthServer now references to `Volo.Abp.Account.Pro.Public.HttpApi` layer. Add this reference to your `AuthServer.csproj` and the `AbpAccountPublicHttpApiModule` dependency to the **AuthServerModule**.
- SwaggerUI is enabled for AuthServer with the name **Account Service**.
- External authentications for Google, Twitter, and Microsoft account are enabled. 
- Added IdentityServer configuration for the non-development environment. Now AuthServer contains deployment configuration for signing credentials.

## Angular Back-Office Application

- Renamed `oAuthConfig` scope **AuthServer** to **AccountService** in environment.ts.

- Added Account remote service to apis in environment.ts:

  ```typescript
  AbpAccountPublic: {
        url: oAuthConfig.issuer,
        rootNamespace: 'AbpAccountPublic',
      },
  ```

## Gateway Changes

- Removed all the microservice and Account module Http.Api layers project references and module dependencies.
- Reflected renaming of `SwaggerWithAuthConfigurationHelper` class to `SwaggerConfigurationHelper`. 
- Extracted ocelot re-route configurations from appsettings.json to **ocelot.json** file.
- Removed extra midware (`app.MapWhen()`) used for dynamic proxying.
- Renamed swagger scope `AuthServer` to `AccountService` in Web gateway.
- Gateways expose swagger endpoints based on the services they reroute. Each service swagger authentication is now individual and the **WebGateway** client can be used to authenticate for all services.

## IdentityServer Data Related Changes

- Renamed **AuthServer** to **AccountService** in both `IdentityServerDataSeeder` located under *IdentityService.HttpApi.Host* and *DbMigrator* to reflect the resource and the scope better.
- IdentityServerDataSeeder creates **multiple redirection URIs** for the Web gateway.
- **IdentityServer configuration** used in `IdentityServerDataSeeder` located under *IdentityService.HttpApi.Host* and *DbMigrator* appsettings.json is updated. Configuration now contains **Clients** and **Resources** instead of just Clients now.
- `CreateSwaggerClientsAsync` method is renamed to **CreateWebGatewaySwaggerClientsAsync**. Only the *WebGateway* client is used for swagger authentication now.

## Shared Project Changes

- Moved swagger dependency from **Shared.Hosting.Microservice** to **Shared.Hosting.AspnetCore**.
- Removed **SwaggerWithAuthConfigurationHelper** from `Shared.Hosting.Gateways` and added functionality to **SwaggerConfigurationHelper** with the name `ConfigureWithAuth`.

## Microservice Changes

- Added **CORS configuration** to service modules and to appsettings.json for Web and Public-Web gateways.
- Added external provider configuration to IdentityService Http.Api.Host module.
- ProductService.Http.Api.Client is now using static proxying. Infrastructural services do not use this configuration because all the modules they are hosting are already using static proxying. It is also strongly suggested to use static proxying for newly added microservices to the solution.
