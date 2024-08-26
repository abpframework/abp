# Sample Applications

Here, a list of official samples built with ABP.

## eShopOnAbp

Reference microservice solution built with ABP and .NET.

* [Source code](https://github.com/abpframework/eShopOnAbp)

## Event Hub

A reference application built with ABP. It implements the Domain Driven Design with multiple application layers.

* [Source code](https://github.com/abpframework/eventhub)

## Easy CRM

A middle-size CRM application built with ABP. [Click here](easy-crm.md) to see the details.

## Book Store

A simple CRUD application to show basic principles of developing an application with ABP. The same sample was implemented with different technologies and different modules.

### With Open Source Modules

The following samples uses only the open source (free) modules.

* **Book Store: Razor Pages UI & Entity Framework Core**
  * [Tutorial](../tutorials/book-store/part-01.md?UI=MVC&DB=EF)
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/BookStore-Mvc-EfCore)
* **Book Store: Blazor UI & Entity Framework Core**
  * [Tutorial](../tutorials/book-store/part-01.md?UI=Blazor&DB=EF)
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/BookStore-Blazor-EfCore)
* **Book Store: Angular UI & MongoDB**
  * [Tutorial](../tutorials/book-store/part-01.md?UI=NG&DB=Mongo)
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/BookStore-Angular-MongoDb)
* **Book Store: Modular application (Razor Pages UI & EF Core)**
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/BookStore-Modular)

### With Pro Modules

The following samples uses the pro modules.

- **Book Store: Razor Pages (MVC) UI & Entity Framework Core**
  - [Tutorial](../tutorials/book-store/part-01.md?UI=MVC&DB=EF)
  - [Download the source code](https://abp.io/Account/Login?returnUrl=/api/download/samples/bookstore-mvc-ef)
- **Book Store: Blazor UI & Entity Framework Core**
  - [Tutorial](../tutorials/book-store/part-01.md?UI=Blazor&DB=EF)
  - [Download the source code](https://abp.io/Account/Login?returnUrl=/api/download/samples/bookstore-blazor-efcore)
- **Book Store: Angular UI & MongoDB**
  - [Tutorial](../tutorials/book-store/part-01.md?UI=NG&DB=Mongo)
  - [Download the source code](https://abp.io/Account/Login?returnUrl=/api/download/samples/bookstore-angular-mongodb)

## Other Samples

* **Event Organizer**: A sample application to create events (meetups) and allow others to register the events. Developed using EF Core and Blazor UI.
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/EventOrganizer)
  * [Article](https://abp.io/community/articles/creating-an-event-organizer-application-with-the-blazor-ui-wbe0sf2z)
* **Entity Framework Migrations**: A solution to demonstrate how to split your application into multiple databases each database contains different modules.
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/EfCoreMigrationDemo)
  * [EF Core database migrations document](../framework/data/entity-framework-core/migrations.md)
* **SignalR Demo**: A simple chat application that allows to send and receive messages among authenticated users.
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/SignalRDemo)
  * [SignalR Integration document](../framework/real-time/signalr.md)
* **Real Time Messaging In A Distributed Architecture** (using SingalR & RabbitMQ)
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/SignalRTieredDemo)
  * [Article](https://abp.io/community/articles/real-time-messaging-in-a-distributed-architecture-using-abp-framework-singalr-rabbitmq-daf47e17)
* **Dashboard Demo**: A simple application to show how to use the widget system for the ASP.NET Core MVC UI.
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/DashboardDemo)
  * [Widget documentation](../framework/ui/mvc-razor-pages/widgets.md)
* **RabbitMQ Event Bus Demo**: A solution consists of two applications communicating to each other via distributed events with RabbitMQ integration.
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/RabbitMqEventBus)
  * [Distributed event bus document](../framework/infrastructure/event-bus/distributed)
  * [RabbitMQ distributed event bus integration document](../framework/infrastructure/event-bus/distributed/rabbitmq.md)
* **Text Templates Demo**: Shows different use cases of the text templating system.
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/TextTemplateDemo)
  * [Text templating documentation](../framework/infrastructure/text-templating)
* **Stored Procedure Demo**: Demonstrates how to use stored procedures, database views and functions with best practices.
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/StoredProcedureDemo)
* **Passwordless Authentication**: Shows how to add a custom token provider to authenticate a user with a link, instead of entering a password.
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/PasswordlessAuthentication)
  * [Article](https://abp.io/community/articles/implementing-passwordless-authentication-with-asp.net-core-identity-c25l8koj)
* **Authentication Customization**: A solution to show how to customize the authentication for ASP.NET Core MVC / Razor Pages applications.
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/Authentication-Customization)
  * Related articles:
    * [Azure Active Directory Authentication](https://abp.io/community/articles/how-to-use-the-azure-active-directory-authentication-for-mvc-razor-page-applications-4603b9cf)
    * [Customize the Login Page](https://abp.io/community/articles/how-to-customize-the-login-page-for-mvc-razor-page-applications-9a40f3cd)
    * [Customize the SignIn Manager](https://abp.io/community/articles/how-to-customize-the-signin-manager-3e858753)
* **GRPC Demo**: Shows how to add a gRPC service to an ABP based web application and consume it from a console application.
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/GrpcDemo)
* **Telerik Blazor Integration**: Shows how to install and use Telerik Blazor components with ABP.
  * [Article](https://abp.io/community/articles/how-to-integrate-the-telerik-blazor-components-to-the-abp-blazor-ui-q8g31abb)
* **Angular Material Integration**: Implemented the web application tutorial using the Angular Material library.
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/AcmeBookStoreAngularMaterial)
  * [Article](https://abp.io/community/articles/using-angular-material-components-with-the-abp-framework-af8ft6t9)
* **DevExtreme Angular Component Integration**: How to install and use DevExtreme components in the ABP Angular UI.
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/DevExtreme-Angular)
  * [Article](https://abp.io/community/articles/using-devextreme-angular-components-with-the-abp-framework-x5nyvj3i)
* **DevExtreme MVC / Razor Pages Component Integration**: How to install and use DevExtreme components in the ABP MVC / Razor Pages UI.
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/DevExtreme-Mvc)
  * [Article](https://abp.io/community/articles/using-devextreme-components-with-the-abp-framework-zb8z7yqv)
* **Syncfusion Blazor Integration**: Shows how to install and integrate Syncfusion UI the ABP Blazor UI.
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/SyncfusionSample)
  * [Article](https://abp.io/community/articles/using-syncfusion-components-with-the-abp-framework-5ccvi8kc)
* **Empty ASP.NET Core Application**: The most basic ASP.NET Core application with ABP installed.
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/BasicAspNetCoreApplication)
  * [Documentation](../get-started/empty-aspnet-core-application.md)
* **Using Elsa Workflow with ABP**: Shows how to use the Elsa Core workflow library within an ABP-based application. 
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/ElsaDemo)
  * [Article](https://abp.io/community/articles/using-elsa-workflow-with-the-abp-framework-773siqi9)
