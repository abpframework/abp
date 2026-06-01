```json
//[doc-seo]
{
    "Description": "Explore a variety of ABP Framework samples, complete with live demos, source code, and tutorials to enhance your development skills!"
}
```

# ABP Samples

This document provides a list of samples built with ABP. Each sample is briefly explained below, along with its live demo (if available), source code, and tutorial links (where applicable).

## Event Hub

A reference application built with ABP. It implements the Domain Driven Design with multiple application layers.

* [Live demo](https://www.openeventhub.com/)
* [Source code](https://github.com/abpframework/eventhub)

![samples-event-hub](../images/samples-eventhub.png)

## eShopOnAbp

> ⚠️ **Important Notice**  
> This project, "eShopOnAbp," is outdated. It served as a reference project for microservice architecture using the ABP Framework, but we now recommend using the [ABP Microservice Solution Template](https://abp.io/docs/latest/solution-templates/microservice) for new projects.

Reference microservice solution built with ABP and .NET.

* [Source code](https://github.com/abpframework/eShopOnAbp)

![eshoponabp](../images/samples-eshoponabp.png)

## CMS Kit Demo

A minimal example website built with the [CMS Kit module](../modules/cms-kit/index.md).

* [Live demo](https://cms-kit-demo.abpdemo.com/)
* [Source code](https://github.com/abpframework/cms-kit-demo)

![samples-cms-kit](../images/samples-cms-kit.png)

## Easy CRM

A middle-size CRM application built with ABP.

* [Live demo](http://easycrm.abp.io/)
* [Click here](easy-crm.md) to see the details and download the source code.

![samples-easy-crm](../images/samples-easycrm.png)

## Book Store

A simple CRUD application to show basic principles of developing an application with ABP. The same sample was implemented with different technologies and different modules:

* **Book Store: Razor Pages UI & Entity Framework Core**
  * [Tutorial](../tutorials/book-store/part-01.md?UI=MVC&DB=EF)
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/BookStore-Mvc-EfCore)
  * [Download source code (with PRO modules) *](https://abp.io/Account/Login?returnUrl=/api/download/samples/bookstore-mvc-ef)
* **Book Store: Blazor UI & Entity Framework Core**
  * [Tutorial](../tutorials/book-store/part-01.md?UI=Blazor&DB=EF)
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/BookStore-Blazor-EfCore)
  * [Download source code (with PRO modules) *](https://abp.io/Account/Login?returnUrl=/api/download/samples/bookstore-blazor-efcore)
* **Book Store: Angular UI & MongoDB**
  * [Tutorial](../tutorials/book-store/part-01.md?UI=NG&DB=Mongo)
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/BookStore-Angular-MongoDb)
  * [Download source code (with PRO modules) *](https://abp.io/Account/Login?returnUrl=/api/download/samples/bookstore-angular-mongodb)
* **Book Store: Modular application (Razor Pages UI & EF Core)**
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/BookStore-Modular)

If you want to create the BookStore application and generate CRUD pages automatically with ABP Suite, please refer to the [Book Store Application (with ABP Suite) tutorial](../tutorials/book-store-with-abp-suite/part-01.md). Also, you can follow the [Mobile Application Development Tutorials](../tutorials/mobile/index.md), if you want to implement the CRUD operations for [MAUI](../tutorials/mobile/maui/index.md) & [React Native](../tutorials/mobile/react-native/index.md) mobile applications.

> **Note:** _Downloading source codes (with PRO modules) \*_ require an active [ABP License](https://abp.io/pricing).

## ModularCRM

A modular monolith application that demonstrates how to create, compose, and communicate between application modules to build a modular web application:

* **ModularCRM: Razor Pages UI & Entity Framework Core**
  * [Tutorial](../tutorials/modular-crm/part-01.md?UI=MVC&DB=EF)
  * [Source code](https://github.com/abpframework/abp-samples/tree/master/ModularCRM)

## CloudCrm

> This tutorial & sample application is suitable for those who have an [ABP Business or a higher license](https://abp.io/pricing).

A microservice solution that shows how to start a new microservice solution, create services and communicate between these services. It's a reference tutorial to learn to use these services from a web application through an API gateway and automatically generate CRUD pages using the ABP Suite tool:

* **CloudCRM: Razor Pages UI & Entity Framework Core**
  * [Tutorial](../tutorials/microservice/part-01.md?UI=MVC&DB=EF)
  * [Download source code](https://abp.io/api/download/samples/cloud-crm-mvc-ef)

## Other Samples

ABP Platform provides many sample applications demonstrating various use cases and integrations. You can:

* Browse all sample applications in the [abp-samples repository](https://github.com/abpframework/abp-samples).
* Read detailed articles and tutorials in the [ABP Community](https://abp.io/community), which are shared by ABP Community & Contributors.