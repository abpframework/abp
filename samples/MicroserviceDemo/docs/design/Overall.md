# Microservice Demo Overall

## Introduction

The goal of this work is to show how to create a complete microservice solution based on the ABP framework.

## Tooling

* **[ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-2.2)** as the web framework.
* **[ABP](https://abp.io)** as the application framework.
* **[Ocelot](https://github.com/ThreeMammals/Ocelot)** as the API Gateway.
* **[IdentityServer4](https://identityserver.io/)** as the authentication server/framework.
* [**Redis**](https://redis.io/) for distributed cache.
* [**RabbitMQ**](https://www.rabbitmq.com/) for distributed messaging.

## The Solution

The solution consists of;

* **MVC applications**:
  * **AuthServer Application**
    * Hosts the [IdentityServer4](https://github.com/abpframework/abp/tree/master/modules/identityserver) module for authentication.
    * Uses an SQL server database and [EF Core](https://abp.io/documents/abp/latest/Entity-Framework-Core) as the ORM.
  * **Public Site Application**
    * Hosts the [docs](https://github.com/abpframework/abp/tree/master/modules/docs) module UI. Uses the docs microservice as backend.
    * Hosts the [blogging](https://github.com/abpframework/abp/tree/master/modules/blogging) module UI. Uses the blogging microservice as backend.
    * Contains a sample page that uses multiple microservice API calls.
    * Authenticates from the AuthServer.
  * **Backend Admin Application**
    * Hosts the [Identity](https://github.com/abpframework/abp/tree/master/modules/identity) module UI for user/role/permission management. Uses the identity microservice as backend.
    * Hosts the blogging admin UI to manage the docs web site. Uses the blogging microservice as backend.
* **Microservices**:
  * **Blogging Microservice**
    * Hosts the [blogging](https://github.com/abpframework/abp/tree/master/modules/blogging) module API.
    * Uses its own [MongoDB](https://abp.io/documents/abp/latest/MongoDB) database.
    * Listens user update events (of the Identity microservice) to update the users stored in its own database.
  * **Docs Microservice**
    * Serves the docs module API.
    * Uses its own SQL Server database.
  * **Identity Microservice**
    * Hosts the [identity](https://github.com/abpframework/abp/tree/master/modules/identity) module API.
    * Shares the same SQL Server database with the AuthServer application.
* **API Gateways**
  * **Public Site Gateway**: Used by the public site application.
  * **Backend Gateway**: Used by the Backend admin application.

## Notes

* Microservices and the AuthServer application use Redis for caching and RabbitMQ for distributed messaging.
* Every microservice is configured for auditing.
* Uses docker-compose.
* Will create sample background jobs for demonstration.