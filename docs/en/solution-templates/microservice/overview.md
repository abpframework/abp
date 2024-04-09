# Microservice Solution: Overview

In this document, you will learn what the Microservice solution template offers to you.

## The Big Picture

***TODO: Prepare a diagram of the fundamental components of the system.***

## Pre-Installed Libraries & Services

All the following **libraries and services** are **pre-installed** and **configured** for both of **development** and **production** environments. After creating your solution, you can **change** to **remove** most of them.

* **[Autofac](https://autofac.org/)** for [Dependency Injection](https://docs.abp.io/en/abp/latest/Dependency-Injection)
* **[Serilog](https://serilog.net/)** with File, Console and Elasticsearch [logging](https://docs.abp.io/en/abp/latest/Logging) providers
* **[Prometheus](https://prometheus.io/)** for collecting metrics
* **[Grafana](https://grafana.com/)** to visualize the collected metrics
* **[Redis](https://redis.io/)** for [distributed caching](https://docs.abp.io/en/abp/latest/Caching) and [distributed locking](https://docs.abp.io/en/abp/latest/Distributed-Locking)
* **[Swagger](https://swagger.io/)** to explore and test HTTP APIs
* **[RabbitMQ](https://www.rabbitmq.com/)** as the [distributed event bus](https://docs.abp.io/en/abp/latest/Distributed-Event-Bus)
* **[YARP](https://microsoft.github.io/reverse-proxy/)** to implement the API Gateways
* **[OpenIddict](https://github.com/openiddict/openiddict-core)** as the in-house authentication server.

## Pre-Configured Features

The following features are built and pre-configured for you in the solution.

* **Authentication** is fully configured based on best practices;
  * **JWT Bearer Authentication** for microservices and applications.
  * **OpenId Connect Authentication**, if you have selected the MVC UI.
  * **Authorization code flow** is implemented, if you have selected a SPA UI (Angular or Blazor WASM).
  * Other flows (resource owner password, client credentials...) are easy to use when you need them.
* **[Permission](https://docs.abp.io/en/abp/latest/Authorization)** (authorization), **[setting](https://docs.abp.io/en/abp/latest/Settings)**, **[feature](https://docs.abp.io/en/abp/latest/Features)** and the **[localization](https://docs.abp.io/en/abp/latest/Localization)** management systems are pre-configured and ready to use.
* **[Background job system](https://docs.abp.io/en/abp/latest/Background-Jobs)** with [RabbitMQ integrated](https://docs.abp.io/en/abp/latest/Background-Jobs-RabbitMq).
* **[BLOB storge](https://docs.abp.io/en/abp/latest/Blob-Storing)** system is installed with the [database provider](https://docs.abp.io/en/abp/latest/Blob-Storing-Database) and a separate database.
* **On-the-fly database migration** system (services automatically migrated their database schema when you deploy a new version)
* Infrastructure dependencies are configured via **[docker-compose](https://docs.docker.com/compose/)** for running the solution in local environment.
* **[Helm](https://helm.sh/)** charts are included to deploy the solution to **[Kubernetes](https://kubernetes.io/)**.
* **[Swagger](https://swagger.io/)** authentication is configured to test the authorized HTTP APIs.
* Configured the **[Inbox & Outbox patterns](https://docs.abp.io/en/abp/latest/Distributed-Event-Bus#outbox-inbox-for-transactional-events)** for [distributed event bus](https://docs.abp.io/en/abp/latest/Distributed-Event-Bus).

## Fundamental Modules

The following modules are pre-installed and configured for the solution:

* **[Account](../../modules/account.md)** to authenticate users (login, register, two factor auth, etc)
* **[Identity](../../modules/identity.md)** to manage roles and users
* **[OpenIddict](https://docs.abp.io/en/abp/latest/Modules/OpenIddict)** (the core part) to implement the OAuth authentication flows

In addition these, [Feature Management](https://docs.abp.io/en/abp/latest/Modules/Feature-Management), [Permission Management](https://docs.abp.io/en/abp/latest/Modules/Permission-Management) and [Setting Management](https://docs.abp.io/en/abp/latest/Modules/Setting-Management) modules are pre-installed as they are the fundamental feature modules of the ABP Framework.

## Optional Modules

The following modules are optionally included in the solution, so you can select the ones you need:

* **[Audit Logging](../../modules/audit-logging.md)** (with its own microservice)
* **[GDPR](../../modules/gdpr.md)** (with its own microservice)
* **[Language Management](../../modules/language-management.md)**
* **[OpenIddict (Management UI)](../../modules/openiddict.md)**
* **[SaaS](../../modules/saas.md) (Multi-Tenancy)** (with its own microservice)
* **[Text Template Management](../../modules/text-template-management.md)**

## UI Theme

The **[LeptonX theme](https://leptontheme.com/)** is pre-configured for the solution. You can select one of the color palettes (System, Light or Dark) as default, while the end-user dynamically change it on the fly.

## Other Options

Microservice startup template asks for some preferences while creating your solution.

### Database Providers

There are two database provider options are provided on a new microservice solution creation:

* **[Entity Framework Core](https://docs.abp.io/en/abp/latest/Entity-Framework-Core)** with SQL Server, MySQL and PostgreSQL DBMS options. You can [switch to anther DBMS](https://docs.abp.io/en/abp/latest/Entity-Framework-Core-Other-DBMS) manually after creating your solution.
* **[MongoDB](https://docs.abp.io/en/abp/latest/MongoDB)**

### UI Frameworks

The solution comes with a main web application with the following UI Framework options:

* **None** (doesn't include a web application to the solution)
* **Angular**
* **MVC / Razor Pages UI**
* **Blazor WebAssembly**
* **Blazor Server**
* **MAUI with Blazor (Hybrid)**

### The Mobile Application

If you prefer, the solution includes a mobile application with its dedicated API Gateway. The mobile application is fully integrated to the system, implements authentication (login) and other ABP features, and includes a few screens that you can use and take as example. The following options are available:

* **None** (doesn't include a mobile application to the solution)
* **MAUI**
* **React Native**

### Multi-Tenancy & SaaS Module

The **[SaaS module](../../modules/saas.md)** is included as an option. When you select it, the **[multi-tenancy](https://docs.abp.io/en/abp/latest/Multi-Tenancy)** system is automatically configured. Otherwise, the system will not include any multi-tenancy overhead.

## Next

* [Solution Structure](solution-structure.md)

## See Also

* [Quick Start: Creating a Microservice Solution with ABP Studio](../../quick-starts/microservice.md)
