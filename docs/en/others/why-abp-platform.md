# Why ABP Platform?

This document aims to answer the big question:

> "Why should you use the ABP Platform instead of creating a new solution from scratch?"

The document introduces the challenges of building a modern software solution and explains how ABP addresses these challenges.

See also https://abp.io/why-abp-io

## Creating a New Solution

When you need to start a new solution, there are **a lot of questions** you need to ask yourself, and you should spend **a lot of time** before starting to write your **very first business code**.

### Creating an Empty Solution

Even creating an almost-empty solution is challenging;

*   How do you **organize your codebase** across projects?
*   What are the **layers** and how do they interact?
*   How do you **integrate** to 3rd-party library and systems?
*   How to set up the automated **tests**?

ABP provides a **well-architected**, **layered** and **production-ready** [startup solution](../solution-templates/layered-web-application) based on the [Domain Driven Design](../framework/architecture/domain-driven-design) principles. The solution also includes a pre-configured **unit** and **integration** [test](../testing/overall.md) projects for each layer.

Creating such a solution structure requires a good **architectural experience** and a **significant time to prepare** it. ABP provides it in just one minute.

### Common Libraries

Which **libraries** should you use to implement **common requirements**? The software development ecosystem is highly dynamic and it is **hard to follow** the latest tools, libraries, trends and approaches.

ABP **pre-integrates** the **popular**, **mature** and **up-to-date libraries** into the solution. You don't spend time integrating them and talking to each other. They properly work out of the box.

### UI Theme & Layout

When it comes to the UI, there are a lot of challenges, including preparing a foundation to create a **responsive**, **modern** and **flexible** UI kit with a consistent look & feel and tons of features (like left/top navigation menu, header, toolbar, footer, widgets and so.).

Even if you buy a pre-built theme, **integrating** it into your solution may take **days of development**. Upgrading such a theme is another problem. Most of the time, the theme's HTML/CSS structure is mixed with your UI code, and it is not easy to **upgrade** or **change** the theme later.

ABP provides a [theming](../framework/ui/mvc-razor-pages/theming.md) system that makes your UI code independent from the theme. Themes are isolated, and they are **NuGet/NPM packages**. Installing or upgrading a theme is just a minute. While you can build your theme (or integrate an existing theme), ABP offers **professional and modern** [themes](https://abp.io/themes).

> There are also UI **component providers** (like Telerik and DevExpress). But they only provide individual components. You are responsible for creating your own **layout system**. You **can use** such libraries in your ABP-based solutions just like in any other project.

### Test Infrastructure

Preparing a robust test environment takes time. You need to **setup test projects** in your solution, select the tools, **mock the services and database**, create the required base classes and utility services to reduce repeating code in the tests and so on.

ABP Startup Templates comes with the test projects already configured for you, and you can immediately write your first unit or integration test code on day 1. Writing unit tests is very clear with the ABP; see [how to write tests](../testing/overall.md).

### Coding Standards & Training

Once you create the development-ready solution, you typically need to **train the developers** to explain the system and develop it with the same **conventions** in a **standard** and consistent way. Even if you train the developers, it is **hard to prepare and maintain your documentation**. Over time, every developer will write the code differently, and coding standards will begin to diverge.

ABP solution is already **well-defined** and **well-documented**. [Tutorials](../tutorials/book-store/part-01.md) and [best practice guides](../framework/architecture/best-practices) clearly explain how to make development on an ABP project.

### Keeping Your Solution Up to Date

After you start your development, you must keep track of the new versions of the libraries you use for upgrades & patches. We regularly update all packages to the latest versions and test them before the stable release. When you update the ABP, all its dependencies are upgraded to edge technology.

`abp update` [CLI](../cli) command automatically discovers and upgrades all ABP-dependant NuGet and NPM packages in a solution. With ABP, it is easier to stay with the latest versions.

## Don't Repeat Yourself!

Creating a base solution takes significant time and requires well architectural experience. However, this is just the beginning! As you start developing, you will likely have to write lots of repetitive code; that would be great if all this could be handled automatically.

ABP automates and simplifies repeating code as much as possible by following the convention over configuration principle. However, it doesn't restrict you when you need to switch to manual gear. The control is always in your hands.

### Authentication

Single Sign On, Active Directory / LDAP Integration, OpenIddict integration, social logins, two-factor authentication, forgot/reset password, email activation, new user registration, password complexity control, locking account on failed attempts, showing failed login attemps ... etc. We know that all these generic requirements are familiar to you. You are not alone!

ABP and the commercial version provide all these standard stuff pre-implemented for you as a re-usable account module. You just enable and configure what you need.

### Cross-Cutting Concerns

Cross-Cutting Concerns are the fundamental repeating logic that should be implemented for each use case. Some examples;

*   Starting **transactions**, committing on success and rollback on errors.
*   **Handling and reporting exceptions**, returning a proper error response to the clients and handling error cases on the client-side.
*   Implementing **authorization** and **validation**, returning proper responses and handling these on the client-side.

ABP automates or simplifies all the common cross-cutting concerns. You only write code that matters for your business and ABP handles the rest by conventions.

### Architectural Infrastructure

You typically need to build **infrastructure** to implement your **architecture properly**. For example, you generally implement the **Repository** pattern. You define some **base classes** to **simplify** and **standardize** to create entities, services, controllers and other objects. ABP provides all these and more out of the box. It is mature and well-documented.

### Enterprise Application Requirements

There are a lot of requirements you repeatedly implement in every business application;

* Detailed **permission system** and managing permissions on the UI based on roles and users.
* Writing **audit logs** and **entity histories** to track when a user modifies a database record.
* Make your entities **soft delete**, so they are marked as deleted instead of physically deleting from the database and automatically filtering deleted entities on your queries.
* Creating abstractions and wrappers to **consume your backend APIs** from the frontend code.
* Enqueuing and executing **background jobs**.
* Handling multiple **time zones** in a global system.
* Sharing **validation**, **localization**, **authorization** logic between server and client.

There is much more. ABP provides infrastructure to implement such requirements easily. Again, you don't spend your valuable time to re-implement all these again and again.

### Generating Initial Code & Tooling

[ABP Suite](https://abp.io/tools/suite) can generate a full-stack CRUD page for your entities in seconds. The generated code is layered and clean. All the standard validation and authorization requirements are implemented. Plus, unit test classes are generated. Once you get a fully running page, you can modify it according to your business requirements.

### Integrating to 3rd-Party Libraries and Systems

Most libraries are designed as **low level**, and you typically do some work to integrate them properly **without repeating** the same integration and configuration code everywhere in your solution.

For example, assume you must use **RabbitMQ** to implement your distributed event bus. All you want to do is; send a message to a queue and handle the incoming messages. But you need to understand **messaging patterns**, queue and **exchange details**. To write an efficient code, you must create a **pool** to manage connections, clients and channels. You also must deal with **exceptions**, ACK messages, **re-connecting** to RabbitMQ on failures and more.

ABP's RabbitMQ Distributed Event Bus integration abstracts all these details. You send and receive messages without the hustle and bustle. Do you need to write low-level code? No problem, you can always do that. ABP doesn't restrict you when you need to use low-level features of the library you are using.

### Tons of Features

Tag helpers, dynamic forms, BLOB storing system and many other ABP features help you keep DRY (don't repeat yourself) and focus on your business.

### Why Not Build Your Own Framework?

All the infrastructure, even in the most simple way, takes a lot of time to **build**, **maintain** and **document**. It gets bigger over time, and it becomes hard to maintain it in your solution. Separating these into a re-usable project is the starting point for building your own **internal framework**.

Building, documenting, training and maintaining an internal framework is really hard. If you don't have an experienced, dedicated framework team, your internal framework rapidly becomes an undocumented legacy code that no one can understand and maintain anymore. On the other hand, these frameworks are generally developed by one or two developers in the team. And these fellows are becoming a knowledge silo. It is good for them but bad for the company because they are the project's single point of failure -[SPOF](https://en.wikipedia.org/wiki/Single_point_of_failure)-. Once they leave the company, the project dramatically goes down.

## Architectural Infrastructure

SaaS applications, modular or microservice systems are most used enterprise software models. Building such systems not only requires a good understanding and experience, but also requires a strong software infrastructure. Otherwise, you will find yourself spending a great effort to support these architectural details in your codebase.

### Modularity

Building a truly modular system is not easy! All the aspects of the system (database, entities, APIs, UI pages/components) can be split into modules, and each module can be re-usable without others. The plain ASP.NET Core doesn't provide such a modular architecture. If you need it, you should think about it from scratch.

The ABP is born to be a modular application development structure. Every feature in the framework is developed to be compatible with modularity. Documentation and guides explain how to develop re-usable modules in a standard way.

### SaaS / Multi-Tenancy

[Multi-Tenancy](../framework/architecture/multi-tenancy) is a common way to implement SaaS systems. However, implementing a consistent multi-tenant infrastructure may become complicated.

ABP provides a complete multi-tenant infrastructure and abstract complexity from your business code. Your application code will be mostly multi-tenancy aware while the ABP automatically isolates the database, cache and other details of the tenants from each other. It supports single database, per tenant database and hybrid approaches. It properly configures the libraries like Microsoft Identity and OpenIddict, which are not normally multi-tenancy compatible.

### Microservices

Building a microservice system requires many infrastructure details: Authenticating and authorizing applications and microservices and implementing asynchronous messaging and synchronous (Rest/GRPC) communication patterns between microservices are the most fundamental issues.

The ABP provides services, [guides](../framework/architecture/microservices), and samples to help you implement your microservice solution using the industry standard tools.

ABP even goes one step further and provides a complete [startup template](../solution-templates/microservice) to kick-start your microservice solution.

## Pre-Built Modules

All of us have similar but slightly different business requirements. However, we all should re-invent the wheel since no one's code can directly work in our solution. They are all embedded parts of a larger solution.

ABP [modules](https://abp.io/modules) provides a lot of **re-usable application modules** like payment, chat, file management, audit log reporting... etc. All of these modules are easily installed into your solution and directly work. We are constantly adding more modules.

All modules are designed as customizable for your business requirements. If you need complete control, you can download the full source code of any module and completely customize based on your specific business requirements.

## ABP Community

Finally, being in a big community where everyone follows similar coding styles and principles and shares a common infrastructure brings power when you have troubles or need help with design decisions. Since we write code similarly, we can help each other much better. ABP is a community-backed project with more than 10K stars on GitHub. 

It is easy to share code or even re-usable libraries between ABP developers. A code snippet that works for you will also work for others. There are a lot of samples and tutorials that you can directly implement for your application.

When you hire a developer who worked before with the ABP architecture will immediately understand your solution and start development in a very short time.

## Questions On Your Mind

### Is the learning curve high?

The learning curve is much lower than not using the ABP. That may sound surprising to you, but let's explain it;

ABP creates a full stack, production-ready, working solution for you in seconds. Many of the real-life problems are already solved and many fine tune configurations are already applied for the ASP.NET Core and the other used libraries. If you start from scratch, you will experience and learn all these details yourself to truly implement your solution.

ABP uses the industry standard frameworks, libraries and systems you already know (or need to learn to build a real-world product) like Angular, Blazor, MAUI, EF Core, AutoMapper, OpenIddict, Bootstrap, Redis, SignalR... etc. So, all your knowledge is directly re-usable with the ABP. ABP even simplifies using these libraries and systems and solves the integration problems. If you don't know these tools now, learning them will be easier within the ABP.

ABP provides an excellent infrastructure to apply DDD principles and other best practices. It provides a lot of sweet abstractions and automation to reduce the repeating code. However, it doesn't force you to use or apply all these. A common mistake is to see that ABP has a lot of features, and it is hard to learn all of them. Having a lot of features is an advantage when you come to the point that you need them. However, you don't need to know a feature until you need it, and you can continue with the development approach you are used to. You can still write code as you are used to as if ABP doesn't provide all these benefits. Learning the ABP infrastructure is progressive. You will love it whenever you learn a new feature but can continue developing without knowing its existence.

Finally, learning an ABP feature is much easier than building the same feature yourself. We've done the hard work for you and documented it well. You will enjoy and love it as you learn.

### Is it overhead for simple CRUD applications?

ABP simplifies building CRUD applications because CRUD applications are more suitable to automate.

ABP provides default repositories, CRUD application service base class, pre-built DTO classes, simplified client-to-server communication approach, UI components, helpers, and more to build CRUD functionalities for your applications in the production quality.

ABP Suite even automatically creates production-level CRUD functionality when you define the properties of your entity.

Finally, most of the applications and systems are considered simple at the beginning, but they grow over time. Once your application grows and you need to implement some advanced requirements, ABP will help you even more.

### Is it sufficient for complex systems?

ABP is designed to solve real-world problems and build complex systems. It provides a lot of infrastructure requirements already implemented for your system and provides lots of integrations to enterprise systems and tools.

We definitely know that there is no limit to the **mountain of complexity** and no one can provide everything to you. **ABP Platform is a helicopter that puts you at a high mountain point and provides many useful tools to help you climb the rest yourself.**

### What if I want to customize it?

Customization has different types and levels;

* ABP itself is highly customizable. You can almost replace and override any service. 3rd-party dependencies are abstracted, and generally, multiple alternatives are provided. You can implement your integrations to extend the framework.
* Modules are designed to be customizable and extensible from the database to the user interface. The Framework provides a [standard extensibility model](../framework/architecture/modularity/extending/customizing-application-modules-guide.md) implemented wherever possible.
* Modules are designed as layered and compatible with [different architectures](../framework/architecture/best-practices). They can be used as part of a monolithic application or deployed as standalone microservices with their own databases.
* Modules are independent of each other. You can remove one module without touching others.

For most of the applications, these are more than necessary. However, it can be different from the code in your solution. You can always replace a module with its source code when you want to change it completely. For ABP modules, we always provide a source code option.

### What if I need to bypass ABP abstractions?

ABP provides a lot of abstractions on top of the ASP.NET Core and some other libraries to simplify development and increase your productivity. However, it is always possible to not use ABP abstractions and directly use the underlying API.

For example, ABP provides repositories to simplify data access. However, if you prefer, you can directly access the DbContext and use any Entity Framework Core API, as you can do in any .NET project.

### Can I use other database systems?

ABP provides Entity Framework Core (you can use almost any relational database system) and MongoDB integrations out of the box. In addition, it provides EF Core compatible Dapper integration. All pre-built modules have EF Core and MongoDB options.

If you need to use any other database system, you can use its libraries and APIs. The ABP doesn't have any restrictions here. For example, you can use Cassandra for your application code. If you use a pre-built module, it will probably not have a Cassandra integration package. You have two options here: You can implement the Cassandra integration yourself (by implementing a few repository interfaces defined by the module) or let the module continue using its database. Multiple database systems can run in a single application without any problem.

### Does ABP slows my application?

ABP automates many common tasks you would manually implement if you didn't use it. Some examples are exception handling, validation, authorization, unit of work (and transaction management), audit logging... etc.

If you'd written all these code yourself and if they run in every HTTP request, wouldn't it make your application code a little slower? Yes, the ABP will make your application slightly slower. The good news is that the overhead is not much, and the ABP features are already optimized. If you start to disable these ABP features, you will see the overhead decreases. However, you typically need these features; the slight performance difference is acceptable. As said before, if you wrote these yourself, it would have a similar performance overhead.

So, for most systems, the performance overhead can be safely ignored. For others, you can disable the ABP features you don't need to.

### Is ABP a CMS?

**ABP is not a CMS** (Content Management System). It is a **generic business application development framework**. The Framework doesn't make any assumption for CMS or any applications. It is well-layered so that the core modules are Web or ASP.NET Core independent and can be used to develop console applications, background services or any .NET compatible application.

By the way, the ABP provides modularity, and a CMS module or CMS system can be developed on top of it. We are already providing a [CMS Kit module](../modules/cms-kit) that provides some common CMS primitives so that you can build your CMS on top of it.

## See Also

* **[ABP Community Talk 2023.2: Why Should You Use the ABP as a .NET Developer?](https://abp.io/community/events/why-should-you-use-the-abp-framework-as-a-.net-developer-xd489a48)**
