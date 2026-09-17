```json
//[doc-seo]
{
    "Description": "Explore microservice architecture and learn how it enhances modularity, scalability, and continuous delivery for efficient application development."
}
```

# Microservice Architecture

````json
//[doc-nav]
{
  "Next": {
    "Name": "Microservice Solution Template",
    "Path": "solution-templates/microservice/index"
  }
}
````

*"Microservices are a software development technique—a variant of the **service-oriented architecture** (SOA) architectural style that structures an application as a collection of **loosely coupled services**. In a microservices architecture, services are **fine-grained** and the protocols are **lightweight**. The benefit of decomposing an application into different smaller services is that it improves **modularity**. This makes the application easier to understand, develop, test, and become more resilient to architecture erosion. It **parallelizes development** by enabling small autonomous teams to **develop, deploy and scale** their respective services independently. It also allows the architecture of an individual service to emerge through **continuous refactoring**. Microservices-based architectures enable **continuous delivery and deployment**."*

— [Wikipedia](https://en.wikipedia.org/wiki/Microservices)

## Introduction

One of the major goals of the ABP is to provide a convenient infrastructure to create microservice solutions. To make this possible,

* Provides a [module system](../modularity/basics.md) that allows you to split your application into modules where each module may have its own database, entities, services, APIs, UI components/pages... etc.
* Offers an [architectural model](../best-practices/module-architecture.md) to develop your modules to be compatible to microservice development and deployment.
* Provides [best practices guide](../best-practices) to develop your module standards-compliance.
* Provides base infrastructure to implement [Domain Driven Design](../domain-driven-design) in your microservices.
* Provide services to [automatically create REST-style APIs](../../api-development/auto-controllers.md) from your application services.
* Provide services to [automatically create C# API clients](../../api-development/dynamic-csharp-clients.md) that makes easy to consume your services from another service/application.
* Provides a [distributed event bus](../../infrastructure/event-bus) to communicate your services.
* Provides many other services to make your daily development easier.

## ABP Studio for Microservice Development

[ABP Studio](../../../studio/overview.md) is a comprehensive desktop application that significantly simplifies microservice solution development and management. It provides powerful tools specifically designed for distributed systems:

### Solution Runner

The [Solution Runner](../../../studio/running-applications.md) allows you to run all your microservices with a single click. You can create different profiles to organize services based on your team's needs. For example, `team-1` might only need to run the *Administration* and *Identity* services, while `team-2` works with *SaaS* and *Audit Logging* services. This approach saves resources and speeds up development by allowing each team to run only the services they need.

### Kubernetes Integration

The [Kubernetes Integration](../../../studio/kubernetes.md) panel enables you to deploy your microservices to a Kubernetes cluster and manage them directly from ABP Studio. Key features include:

* **Deploy to Kubernetes**: Build Docker images and install Helm charts with a few clicks.
* **Intercept Services**: Debug and develop specific services locally while the rest of the system runs in Kubernetes. This eliminates the need to run all microservices on your local machine.
* **Redeploy Charts**: Quickly redeploy individual services after making changes.
* **Connect to Cluster Resources**: Access databases, message queues, and other infrastructure services running in the cluster.

### Application Monitoring

The [Application Monitoring](../../../studio/monitoring-applications.md) area provides a centralized view of all your running microservices:

* **HTTP Requests**: View all HTTP requests across services with detailed information including headers, payloads, and response times.
* **Distributed Events**: Monitor all distributed events sent and received by your services, making it easy to debug inter-service communication.
* **Exceptions**: Track exceptions thrown by any service in real-time.
* **Logs**: Access logs from all services in a single place with filtering capabilities.
* **Built-in Browser**: Browse and test your APIs without leaving ABP Studio.

### Creating New Microservices

ABP Studio's [Solution Explorer](../../../studio/solution-explorer.md) makes it easy to [add new microservices to your solution](../../../solution-templates/microservice/adding-new-microservices). Right-click on the `services` folder and select *Add* -> *New Module* -> *Microservice*. ABP Studio will:

* Create the microservice with proper project structure.
* Configure database connections and migrations.
* Set up authentication and authorization.
* Integrate with API gateways.
* Configure distributed event bus connections.
* Add the service to Kubernetes Helm charts.

## Microservice for New Applications

One common advice to start a new solution is **always to start with a monolith**, keep it modular and split into microservices once the monolith becomes a problem. This makes your progress fast in the beginning especially if your team is small and you don't want to deal with challenges of the microservice architecture. 

However, developing such a well-modular application can be a problem since it is **hard to keep modules isolated** from each other as you would do it for microservices (see [Stefan Tilkov's article](https://martinfowler.com/articles/dont-start-monolith.html) about that). Microservice architecture naturally forces you to develop well isolated services, but in a modular monolithic application it's easy to tightly couple modules to each other and design **weak module boundaries** and API contracts.

ABP can help you in that point by offering a **microservice-compatible, strict module architecture** where your module is split into multiple layers/projects and developed in its own VS solution completely isolated and independent from other modules. Such a developed module is a natural microservice yet it can be easily plugged-in a monolithic application. See the [module development best practice guide](../best-practices) that offers a **microservice-first module design**. All [standard ABP modules](https://github.com/abpframework/abp/tree/master/modules) are developed based on this guide. So, you can use these modules by embedding into your monolithic solution or deploy them separately and use via remote APIs. They can share a single database or can have their own database based on your simple configuration.

## Microservice Solution Template

ABP provides a pre-architected and production-ready microservice solution template that includes multiple services, API gateways and applications well integrated with each other. This template helps you quickly start building distributed systems with common microservice patterns.

See the [Microservice Solution Template](../../../solution-templates/microservice/index.md) documentation for details.

## Tutorials

For a hands-on experience, follow the [Microservice Development Tutorial](../../../tutorials/microservice/index.md) that guides you through:

* Creating the initial microservice solution
* Adding new microservices (Catalog and Ordering services)
* Building CRUD functionality
* Implementing HTTP API calls between services
* Using distributed events for asynchronous communication

## See Also

* [Get Started: Microservice Solution](../../../get-started/microservice.md)
* [Microservice Solution Template](../../../solution-templates/microservice/index.md)
* [Microservice Development Tutorial](../../../tutorials/microservice/index.md)
* [ABP Studio Overview](../../../studio/overview.md)
