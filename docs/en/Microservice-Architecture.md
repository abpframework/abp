# Microservice Architecture

*"Microservices are a software development technique—a variant of the **service-oriented architecture** (SOA) architectural style that structures an application as a collection of **loosely coupled services**. In a microservices architecture, services are **fine-grained** and the protocols are **lightweight**. The benefit of decomposing an application into different smaller services is that it improves **modularity**. This makes the application easier to understand, develop, test, and become more resilient to architecture erosion. It **parallelizes development** by enabling small autonomous teams to **develop, deploy and scale** their respective services independently. It also allows the architecture of an individual service to emerge through **continuous refactoring**. Microservices-based architectures enable **continuous delivery and deployment**."*

— [Wikipedia](https://en.wikipedia.org/wiki/Microservices)

## Introduction

One of the major goals of the ABP framework is to provide a convenient infrastructure to create microservice solutions. To make this possible,

* Provides a [module system](Module-Development-Basics.md) that allows you to split your application into modules where each module may have its own database, entities, services, APIs, UI components/pages... etc.
* Offers an [architectural model](Best-Practices/Module-Architecture.md) to develop your modules to be compatible to microservice development and deployment.
* Provides [best practices guide](Best-Practices/Index.md) to develop your module standards-compliance.
* Provides base infrastructure to implement [Domain Driven Design](Domain-Driven-Design.md) in your microservices.
* Provide services to [automatically create REST-style APIs](API/Auto-API-Controllers.md) from your application services.
* Provide services to [automatically create C# API clients](API/Dynamic-CSharp-API-Clients.md) that makes easy to consume your services from another service/application.
* Provides a [distributed event bus](Event-Bus.md) to communicate your services.
* Provides many other services to make your daily development easier.

## Microservice for New Applications

One common advise to start a new solution is **always to start with a monolith**, keep it modular and split into microservices once the monolith becomes a problem. This makes your progress fast in the beginning especially if your team is small and you don't want to deal with challenges of the microservice architecture. 

However, developing such a well-modular application can be a problem since it is **hard to keep modules isolated** from each other as you would do it for microservices (see [Stefan Tilkov's article](https://martinfowler.com/articles/dont-start-monolith.html) about that). Microservice architecture naturally forces you to develop well isolated services, but in a modular monolithic application it's easy to tight couple modules to each other and design **weak module boundaries** and API contracts.

ABP can help you in that point by offerring a **microservice-compatible, strict module architecture** where your module is splitted into multiple layers/projects and developed in its own VS solution completely isolated and independent from other modules. Such a developed module is a natural microservice yet it can be easily plugged-in a monolithic application. See the [module development best practice guide](Best-Practices/Index.md) that offers a **microservice-first module design**. All [standard ABP modules](https://github.com/abpframework/abp/tree/master/modules) are developed based on this guide. So, you can use these modules by embedding into your monolithic solution or deploy them separately and use via remote APIs. They can share a single database or can have their own database based on your simple configuration.

## Microservice Demo Solution

The [sample microservice solution](Samples/Microservice-Demo.md) demonstrates a complete microservice solution based on the ABP framework.
