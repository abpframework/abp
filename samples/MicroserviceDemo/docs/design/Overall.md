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
* **[Serilog](https://serilog.net/)** for logging.

## The Solution

The solution consists of;

* **Applications**:
  * **AuthServer (MVC Application)**
    * Hosts the [IdentityServer4](https://github.com/abpframework/abp/tree/master/modules/identityserver) module for authentication.
    * Uses an SQL server database and [EF Core](https://abp.io/documents/abp/latest/Entity-Framework-Core) as the ORM.
  * **Public Site (MVC Application)**
    * "Our products" page to list the products using the products microservice.
    * Hosts the [blogging](https://github.com/abpframework/abp/tree/master/modules/blogging) module UI. Uses the blogging microservice as backend.
    * Authenticates from the AuthServer.
  * **Backend Admin (MVC Application)**
    * Hosts the [Identity](https://github.com/abpframework/abp/tree/master/modules/identity) module UI for user/role/permission management. Uses the identity microservice as backend.
    * A CRUD page to manage products using the products microservice.
    * Authenticates from the AuthServer.
  * **Test Client (Console Application)**
    * Simply calls a few APIs and writes results to console.
    * Authenticates from the AuthServer.
* **Microservices**:
  * **Blogging Microservice**
    * Hosts the [blogging](https://github.com/abpframework/abp/tree/master/modules/blogging) module API.
    * Uses its own [MongoDB](https://abp.io/documents/abp/latest/MongoDB) database.
    * Listens user update events (of the Identity microservice) to update the users stored in its own database.
  * **Identity Microservice**
    * Hosts the [identity](https://github.com/abpframework/abp/tree/master/modules/identity) module API.
    * Shares the same SQL Server database with the AuthServer application.
  * **Product Microservice**
    * Hosts product management module API.
    * Uses its own SQL Server database.
* **Modules**
  * **Product Management**
    * A simple module that is used to manage products.
    * Uses EF Core & SQL Server for data access.
* **API Gateways**
  * **Public Site Gateway**: Used by the public site application.
  * **Backend Gateway**: Used by the Backend admin application.

## Notes

* Microservices and the AuthServer application use Redis for caching and RabbitMQ for distributed messaging.
* Every microservice is configured for auditing.
* Uses docker-compose.
* Will create sample background jobs for demonstration.