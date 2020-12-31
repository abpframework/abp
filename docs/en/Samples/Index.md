# Sample Applications

Here, a list of official samples built with the ABP Framework. Most of these samples are located under the [abpframework/abp-samples](https://github.com/abpframework/abp-samples) GitHub repository.

### Microservice Demo

A complete solution to demonstrate how to build systems based on the microservice architecture.

* [The complete documentation for this sample](Microservice-Demo.md)
* [Source code](https://github.com/abpframework/abp-samples/tree/master/MicroserviceDemo)
* [Microservice architecture document](../Microservice-Architecture.md)

### Book Store

A simple CRUD application to show basic principles of developing an application with the ABP Framework. The same sample was implemented with different technologies:

* **Book Store: Razor Pages UI & Entity Framework Core**
  * [Tutorial](https://docs.abp.io/en/abp/latest/Tutorials/Part-1?UI=MVC&DB=EF)
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/BookStore-Mvc-EfCore)
* **Book Store: Angular UI & MongoDB**
  * [Tutorial](https://docs.abp.io/en/abp/latest/Tutorials/Part-1?UI=NG&DB=Mongo)
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/BookStore-Angular-MongoDb)
* **Book Store: Modular application (Razor Pages UI & EF Core)**
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/BookStore-Modular)

While there is no Razor Pages & MongoDB combination, you can check both documents to understand it since DB & UI selection don't effect each other.

### Other Samples

* **Event Organizer**: A sample application to create events (meetups) and allow others to register the events. Developed using EF Core and Blazor UI.
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/EventOrganizer)
  * [Article](https://community.abp.io/articles/creating-an-event-organizer-application-with-the-blazor-ui-wbe0sf2z)
* **Entity Framework Migrations**: A solution to demonstrate how to split your application into multiple databases each database contains different modules.
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/EfCoreMigrationDemo)
  * [EF Core database migrations document](../Entity-Framework-Core-Migrations.md)
* **SignalR Demo**: A simple chat application that allows to send and receive messages among authenticated users.
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/SignalRDemo)
  * [SignalR Integration document](../SignalR-Integration.md)
* **Real Time Messaging In A Distributed Architecture** (using SingalR & RabbitMQ)
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/SignalRTieredDemo)
  * [Article](https://community.abp.io/articles/real-time-messaging-in-a-distributed-architecture-using-abp-framework-singalr-rabbitmq-daf47e17)
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
* **Stored Procedure Demo**: Demonstrates how to use stored procedures, database views and functions with best practices.
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/StoredProcedureDemo)
* **Passwordless Authentication**: Shows how to add a custom token provider to authenticate a user with a link, instead of entering a password.
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/PasswordlessAuthentication)
  * [Article](https://community.abp.io/articles/implementing-passwordless-authentication-with-asp.net-core-identity-c25l8koj)
* **Authentication Customization**: A solution to show how to customize the authentication for ASP.NET Core MVC / Razor Pages applications.
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/Authentication-Customization)
  * Related articles:
    * [Azure Active Directory Authentication](https://community.abp.io/articles/how-to-use-the-azure-active-directory-authentication-for-mvc-razor-page-applications-4603b9cf)
    * [Customize the Login Page](https://community.abp.io/articles/how-to-customize-the-login-page-for-mvc-razor-page-applications-9a40f3cd)
    * [Customize the SignIn Manager](https://community.abp.io/articles/how-to-customize-the-signin-manager-3e858753)
* **GRPC Demo**: Shows how to add a gRPC service to an ABP Framework based web application and consume it from a console application.
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/GrpcDemo)
* **Telerik Blazor Integration**: Shows how to install and use Telerik Blazor components with the ABP Framework.
  * [Article](https://community.abp.io/articles/how-to-integrate-the-telerik-blazor-components-to-the-abp-blazor-ui-q8g31abb)
* **Angular Material Integration**: Implemented the web application tutorial using the Angular Material library.
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/AcmeBookStoreAngularMaterial)
  * [Article](https://community.abp.io/articles/using-angular-material-components-with-the-abp-framework-af8ft6t9)
* **DevExtreme Angular Component Integration**: How to install and use DevExtreme components in the ABP Framework Angular UI.
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/DevExtreme-Angular)
  * [Article](https://community.abp.io/articles/using-devextreme-angular-components-with-the-abp-framework-x5nyvj3i)
* **DevExtreme MVC / Razor Pages Component Integration**: How to install and use DevExtreme components in the ABP Framework MVC / Razor Pages UI.
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/DevExtreme-Mvc)
  * [Article](https://community.abp.io/articles/using-devextreme-components-with-the-abp-framework-zb8z7yqv)
* **Empty ASP.NET Core Application**: The most basic ASP.NET Core application with the ABP Framework installed.
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/BasicAspNetCoreApplication)
  * [Documentation](../Getting-Started-AspNetCore-Application.md)