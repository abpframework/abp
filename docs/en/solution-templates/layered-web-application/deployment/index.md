# Deployment

````json
//[doc-params]
{
    "UI": ["MVC", "Blazor", "BlazorServer", "NG"],
    "DB": ["EF", "Mongo"],
    "Tiered": ["Yes", "No"]
}
````

> This document assumes that you prefer to use **{{ UI_Value }}** as the UI framework and **{{ DB_Value }}** as the database provider. For other options, please change the preference on top of this document.

This guide explains how to deploy your application in staging and production environments based on your application architecture;

- [Docker Deployment using Docker Compose](deployment-docker-compose.md)

- [Azure Deployment using Application Service](azure-deployment/azure-deployment.md)

- [IIS Deployment](deployment-iis.md)
