```json
//[doc-seo]
{
    "Description": "Learn to build microservice solutions with ABP Framework, starting from creating services to API gateway integration. Perfect for developers!"
}
```

# Microservice Development Tutorial

````json
//[doc-params]
{
    "UI": ["MVC","Blazor","BlazorServer", "BlazorWebApp", "NG"],
    "DB": ["EF","Mongo"]
}
````

````json
//[doc-nav]
{
  "Next": {
    "Name": "Creating the initial solution",
    "Path": "tutorials/microservice/part-01"
  }
}
````

> This tutorial is suitable for those who have an ABP Business or a higher [license](https://abp.io/pricing).

ABP is designed to be a powerful platform to build microservice solutions. It provides a [microservice solution template](../../solution-templates/microservice/index.md) to easily start a sophisticated microservice solution. All of the [pre-built application modules](../../modules/index.md) are microservice compatible and the core framework fully [supports and simplifies](../../framework/architecture/microservices/index.md) distributed application development.

In this tutorial, you will learn how to start a new microservice solution, create services and communicate between them. You will also learn to use these services from a web application through an API gateway and automatically generate CRUD pages using the [ABP Suite](../../suite/index.md) tool.

## Tutorial Outline

This tutorial is organized as the following parts:

* [Part 01: Creating the initial solution](part-01.md)
* [Part 02: Creating the initial Catalog microservice](part-02.md)
* [Part 03: Building the Catalog microservice](part-03.md)
* [Part 04: Creating the initial Ordering service](part-04.md)
* [Part 05: Building the Ordering service](part-05.md)
* [Part 06: Integrating the services: HTTP API Calls](part-06.md)
* [Part 07: Integrating the services: Using Distributed Events](part-07.md)

## Download the Source Code

After logging in to the ABP website, you can download the source code from {{if UI == "MVC"}} [here](https://abp.io/api/download/samples/cloud-crm-mvc-ef) {{else if UI == "NG"}} [here](https://abp.io/api/download/samples/cloud-crm-ng-ef) {{else if UI == "Blazor"}} [here](https://abp.io/api/download/samples/cloud-crm-blazor-wasm-ef) {{else if UI == "BlazorServer"}} [here](https://abp.io/api/download/samples/cloud-crm-blazor-server-ef) {{else if UI == "BlazorWebApp"}} [here](https://abp.io/api/download/samples/cloud-crm-blazor-webapp-ef) {{end}}.

## See Also

* [Microservice solution template](../../solution-templates/microservice/index.md)