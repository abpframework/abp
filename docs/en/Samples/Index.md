# Sample Applications

Here, a list of official samples built with the ABP Framework. Most of these samples are located under the [abpframework/abp-samples](https://github.com/abpframework/abp-samples) GitHub repository.

### Microservice Demo

A complete solution to demonstrate how to build systems based on the microservice architecture.

* [The complete documentation for this sample](Microservice-Demo.md)
* [Source code](https://github.com/abpframework/abp/tree/dev/samples/MicroserviceDemo)
* [Microservice architecture document](../Microservice-Architecture.md)

### Book Store

A simple CRUD application to show basic principles of developing an application with the ABP Framework. The same sample was implemented with different technologies:

* **Book Store: Razor Pages UI & Entity Framework Core**

  * [Tutorial](https://docs.abp.io/en/abp/latest/Tutorials/Part-1?UI=MVC)
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/BookStore)

* **Book Store: Angular UI & MongoDB**

  * [Tutorial](https://docs.abp.io/en/abp/latest/Tutorials/Part-1?UI=NG)
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/BookStore-Angular-MongoDb)

* **Book Store: Modular application (Razor Pages UI & EF Core)**

  * [Source code](https://github.com/abpframework/abp-samples/tree/master/BookStore-Modular)

While there is no Razor Pages & MongoDB combination, you can check both documents to understand it since DB & UI selection don't effect each other.

### Other Samples

* **Entity Framework Migrations**: A solution to demonstrate how to split your application into multiple databases each database contains different modules.
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/DashboardDemo)
  * [EF Core database migrations document](../Entity-Framework-Core-Migrations.md)
* **SignalR Demo**: A simple chat application that allows to send and receive messages among authenticated users.
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/SignalRDemo)
  * [SignalR Integration document](../SignalR-Integration.md)
* **Dashboard Demo**: A simple application to show how to use the widget system for the ASP.NET Core MVC UI.
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/DashboardDemo)
  * [Widget documentation](../UI/AspNetCore/Widgets.md)
* **RabbitMQ Event Bus Demo**: A solution consists of two applications communicating to each other via distributed events with RabbitMQ integration.
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/RabbitMqEventBus)
  * [Distributed event bus document](../Distributed-Event-Bus.md)
  * [RabbitMQ distributed event bus integration document](../Distributed-Event-Bus-RabbitMQ-Integration.md)
* **Text Templates Demo**: Shows different use cases of the text templating system.
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/TextTemplateDemo)
  * [Text templating documentation](../Text-Templating.md)
* **Authentication Customization**: A solution to show how to customize the authentication for ASP.NET Core MVC / Razor Pages applications.
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/aspnet-core/Authentication-Customization)
  * Related "[How To](../How-To/Index.md)" documents:
    * [Azure Active Directory Authentication](../How-To/Azure-Active-Directory-Authentication-MVC.md)
    * [Customize the Login Page](../How-To/Customize-Login-Page-MVC.md)
    * [Customize the SignIn Manager](../How-To/Customize-SignIn-Manager.md)
* **Empty ASP.NET Core Application**: The most basic ASP.NET Core application with the ABP Framework installed.
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/BasicAspNetCoreApplication)
  * [Documentation](../Getting-Started-AspNetCore-Application.md)
* **Empty Console Application**: The most basic console application with the ABP Framework installed.
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/BasicConsoleApplication)
  * [Documentation](../Getting-Started-Console-Application.md)