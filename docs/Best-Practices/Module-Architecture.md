## Module Architecture Best Practices & Conventions

### Introduction

TODO

### Solution Structure

* **Do** create a separated Visual Studio solution for every module.
* **Do** name the solution as *CompanyName.ModuleName* (for core ABP packages, it's Volo.Abp.ModuleName).
* **Do** develop the module as layered, so it has several projects (packages) those are related to each other.
  * Every package has its own module definition file and explicitly adds [DependsOn] attribute for the depended packages/modules.

#### Domain Layer

* **Do** divide the domain layer into two projects:
  * **Domain shared** package, named as *CompanyName.ModuleName.Domain.Shared*, contains constants, enums and other types those can be safely shared with the all layers of the module. This package can also be shared to 3rd-party clients. It can not contain entities, repositories, domain services or any other business objects.
  * **Domain** package, named as *CompanyName.ModuleName.Domain*, contains entities, repository interfaces, domain service interfaces and their implementations and other domain objects.
    * Domain package depends on the **domain share** package.

#### Application Layer

* **Do** divide the application layer into two projects:
  * **Application contracts** package, named as *CompanyName.ModuleName.Application.Contracts*, contains application service interfaces and related data transfer objects.
    * Application contract package depends on the **domain shared** package.
  * **Application** package, named as *CompanyName.ModuleName.Application*, contains application service implementations.
    * Application package depends on the **domain** and the **application contracts** packages.

#### Infrastructure Layer

* **Do** create a separated integration package for each major library that is planned to be replaceable by another library without effecting the other packages.
* **Do** create a separated integration package for each ORM/database integration like Entity Framework Core and MongoDB.
  * **Do**, for instance, create a *CompanyName.ModuleName.EntityFrameworkCore* package that abstracts the entity framework core integration. ORM integration packages depend on the **domain** package.
  * **Do not** depend on other layers from the ORM/database integration package.

#### HTTP Layer

* **Do** create an **HTTP API** package, named as *CompanyName.ModuleName.HttpApi*, to develop a REST style HTTP API for the module.
  * HTTP API package only depends on the **application contracts** package. It does not depend on the application package.
  * ...
* Do create an **HTTP API Client** package, named as *CompanyName.ModuleName.HttpApi.Client*,...