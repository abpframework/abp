# Azure Deployment using Application Service

````json
//[doc-params]
{
    "UI": ["MVC", "Blazor", "BlazorServer", "NG"],
    "DB": ["EF", "Mongo"],
    "Tiered": ["Yes", "No"]
}
````

> This document assumes that you prefer to use **{{ UI_Value }}** as the UI framework and **{{ DB_Value }}** as the database provider. For other options, please change the preference on top of this document.

## Prerequisites

- An active Azure account. If you don't have one, you can sign up for a [free account](https://azure.microsoft.com/en-us/free/)

- Your ABP **{{ UI_Value }}** project must be ready at a GitHub repository because we will use GitHub Actions to deploy the ABP application to the Azure Web App Service.

- **{{ DB_Value }}** database must be ready to use with your project. If you don't have a database, you can create a new Azure SQL database or Cosmos DB by following the instructions below:

    - [Create a new Azure SQL Database](https://docs.microsoft.com/en-us/azure/azure-sql/database/single-database-create-quickstart?tabs=azure-portal)

    - [Create a new Azure Cosmos DB](https://docs.microsoft.com/en-us/azure/cosmos-db/create-cosmosdb-resources-portal)


### Description of the process in three steps:

1. [Creating an Azure Web App Service Environment ](step1-create-azure-resources)
2. [Customizing the Configuration of Your ABP Application](step2-configuration-application)
3. [Deploying Your Application to Azure Web App Service](step3-deployment-github-action)


## What's next?

- [Creating an Azure Web App Service Environment](step1-create-azure-resources)
