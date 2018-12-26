# Domain Driven Design

## What is DDD?

ABP framework provides an **infrastructure** to make **DDD** based development easier to implement. DDD is [defined in the Wikipedia](https://en.wikipedia.org/wiki/Domain-driven_design) as below:

> **Domain-driven design** (**DDD**) is an approach to software development for complex needs by connecting the implementation to an evolving model. The premise of domain-driven design is the following:
>
> - Placing the project's primary focus on the core domain and domain logic;
> - Basing complex designs on a model of the domain;
> - Initiating a creative collaboration between technical and domain experts to iteratively refine a conceptual model that addresses particular domain problems.

### Layers

ABP follows DDD principles and patterns to achieve a layered application model which consists of four fundamental layers:

- **Presentation Layer**: Provides an interface to the user. Uses the *Application Layer* to achieve user interactions.
- **Application Layer**: Mediates between the Presentation and Domain Layers. Orchestrates business objects to perform specific application tasks. Implements use cases as the application logic.
- **Domain Layer**: Includes business objects and their business rules. This is the heart of the application.
- **Infrastructure Layer**: Provides generic technical capabilities that support higher layers mostly using 3rd-party libraries.

## Contents

* **Domain Layer**
  * [Entities & Aggregate Roots](Entities.md)
  * Value Objects
  * [Repositories](Repositories.md)
  * Domain Services
  * Specifications
* **Application Layer**
  * [Application Services](Application-Services.md)
  * [Data Transfer Objects (DTOs)](Data-Transfer-Objects.md)
  * Unit of Work