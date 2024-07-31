# ABP Architecture

ABP offers an **opinionated architecture** to build enterprise software solutions. In this way, the solution structure and development model is pretty well defined. All the solution templates, tooling, pre-built modules, guides and documentation are compatible to that development model.

While the architecture is opinionated, it is flexible enough to create any kind of application without any restriction. Essentially, anything you can do with a plain .NET application is already possible with an ABP-based application, allowing ABP to be learned gradually and using its features whenever you need it.

ABP's architecture is built on the following principles and infrastructures:

* **[Modularity](./modularity/basics.md)**: ABP is designed to allow to easily build true modular applications and systems. There are many [pre-built official application modules](../../modules) that you can easily install and use in your applications. There are [best practice guides](./best-practices) to explain how to build reusable application modules. Also, the core ABP is a modular framework. It consists of hundreds of [NuGet & NPM packages](https://abp.io/packages), so you use only what you need.
* **[Domain-Driven Design](domain-driven-design)**: ABP offers DDD & clean architecture principles and patterns to build complex software solutions. While it doesn't force you to implement DDD for your application, it provides infrastructure and documentation that helps you a lot when you want to implement it.
* **[Microservices](microservices)**: One of the major goals of ABP is to provide a convenient infrastructure when you need to create microservice solutions. It provides infrastructure to make the distributed communication and coordination easier and automate the details wherever possible.
* **[Multi-Tenancy](multi-tenancy)**: Multi-Tenancy is a widely used architecture to create **SaaS applications** where the hardware and software **resources are shared by the customers** (tenants). ABP provides all the base functionalities to create **multi tenant applications**.

With ABP, you can create all kinds of applications, from simple console applications to CRUD web applications, from modular systems to large-scale microservice solutions.
