# Provisioning an Azure Web App using Terraform

````json
//[doc-params]
{
    "UI": ["MVC", "Blazor", "BlazorServer", "NG"],
    "DB": ["EF", "Mongo"],
    "Tiered": ["Yes", "No"]
}
````

In this tutorial, we'll walk through the steps to provision an Azure Web App using Terraform. Terraform is an open-source infrastructure as a code tool that allows you to define and manage your infrastructure in a declarative way.

## Prerequisites

Before you begin, you'll need the following:

- [Azure account](https://azure.microsoft.com/en-us/free/) 
- [Terraform installed](https://developer.hashicorp.com/terraform/downloads) on your local machine 
- [Azure CLI installed](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli) on your local machine

## Creating a Service Principal for Terraform in Azure

When working with Terraform on Azure, you'll need a "Service Principal" for authentication. A "Service Principal" is an identity created to be used with applications, hosted services, and automated tools to access Azure resources.

[To create a service principal](https://learn.microsoft.com/en-us/azure/developer/terraform/authenticate-to-azure?tabs=bash#create-a-service-principal), run the following command in the Azure CLI:

1. Login to Azure CLI

    Before you begin, make sure you are logged into your Azure account with the Azure CLI:
    ```bash
    az login
    ```

2. Set your Subscription:

    If you have multiple Azure subscriptions, specify the one you intend to use:
    ```bash
    az account set --subscription="YOUR_SUBSCRIPTION_ID"
    ```

3. Create the Service Principal:

    The following command will create a service principal. Replace YOUR_APP_NAME with a suitable name for your application:
    ```bash
    az ad sp create-for-rbac --name "YOUR_APP_NAME" --role contributor --scopes /subscriptions/YOUR_SUBSCRIPTION_ID
    ```
    > Replace `YOUR_SUBSCRIPTION_ID` with your subscription id.

    The output of this command will provide the **appId**, **displayName**, **name**, **password**, and **tenant**. It's crucial to note these values, especially **appId (Client ID)** and **password (Client Secret)**, as you'll need them for Terraform authentication.

4. Specify the service principal credentials in environment variables

    bash:
    ```bash
    export ARM_SUBSCRIPTION_ID="<azure_subscription_id>"
    export ARM_TENANT_ID="<azure_subscription_tenant_id>"
    export ARM_CLIENT_ID="<service_principal_appid>"
    export ARM_CLIENT_SECRET="<service_principal_password>"
    ```
    To execute the ~/.bashrc script, run source ~/.bashrc (or its abbreviated equivalent . ~/.bashrc). You can also exit and reopen Cloud Shell for the script to run automatically.
    Run the following bash command to verify the Azure environment variables:
    ```bash
    . ~/.bashrc
    ```
    powershell:
    ```powershell
    $env:ARM_SUBSCRIPTION_ID="<azure_subscription_id>"
    $env:ARM_TENANT_ID="<azure_subscription_tenant_id>"
    $env:ARM_CLIENT_ID="<service_principal_appid>"
    $env:ARM_CLIENT_SECRET="<service_principal_password>"
    ```
    Run the following PowerShell command to verify the Azure environment variables:
    ```powershell
    gci env:ARM_*
    ```
    > Replace the values with your own.

## Creating a Terraform Configuration

1. Create a new directory for your Terraform configuration files.

2. Create a new file named `main.tf` in the directory and add the following code:

{{if UI == "NG"}}

```terraform
# Configure the Azure provider
terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.0.0"
    }
  }
  required_version = ">= 0.14.9"
}
provider "azurerm" {
  features {}
}

# Create the resource group
resource "azurerm_resource_group" "rg" {
  name     = "demo-angular-web-app-rg"
  location = "westeurope"
}

# Create the Linux App Service Plan
resource "azurerm_service_plan" "appserviceplan" {
  name                = "demo-angular-web-app-plan"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  os_type             = "Linux"
  sku_name            = "B3"
}


resource "azurerm_linux_web_app" "apihost" {
  name                  = "apihost-angular"
  location              = azurerm_resource_group.rg.location
  resource_group_name   = azurerm_resource_group.rg.name
  service_plan_id       = azurerm_service_plan.appserviceplan.id
  https_only            = true
  site_config {
    application_stack {
      dotnet_version = "6.0"
    }
    minimum_tls_version  = "1.2"
  }
}

resource "azurerm_static_site" "angularweb" {
  name                = "angularweb"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
}
```

{{ else if UI == "Blazor" }}

```terraform
# Configure the Azure provider
terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.0.0"
    }
  }
  required_version = ">= 0.14.9"
}
provider "azurerm" {
  features {}
}

# Create the resource group
resource "azurerm_resource_group" "rg" {
  name     = "blazor-app-nontier-rg"
  location = "westeurope"
}

# Create the Linux App Service Plan
resource "azurerm_service_plan" "appserviceplan" {
  name                = "blazor-app-nontier-plan"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  os_type             = "Linux"
  sku_name            = "B3"
}

# Create the web app, pass in the App Service Plan ID

resource "azurerm_linux_web_app" "apihost" {
  name                  = "apihost-blazor"
  location              = azurerm_resource_group.rg.location
  resource_group_name   = azurerm_resource_group.rg.name
  service_plan_id       = azurerm_service_plan.appserviceplan.id
  https_only            = true
  site_config {
    application_stack {
      dotnet_version = "6.0"
    }
    minimum_tls_version  = "1.2"
  }
}
resource "azurerm_static_site" "blazorweb" {
  name                = "blazorweb"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
}
```

{{ else if UI == "BlazorServer" }}

    {{if Tiered == "No"}}

```terraform
# Configure the Azure provider
terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.0.0"
    }
  }
  required_version = ">= 0.14.9"
}
provider "azurerm" {
  features {}
}

# Create the resource group
resource "azurerm_resource_group" "rg" {
  name     = "blazorserver-app-nontier-rg"
  location = "westeurope"
}

# Create the Linux App Service Plan
resource "azurerm_service_plan" "appserviceplan" {
  name                = "blazorserver-app-nontier-plan"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  os_type             = "Linux"
  sku_name            = "B3"
}

# Create the web app, pass in the App Service Plan ID
resource "azurerm_linux_web_app" "authserver" {
  name                  = "authserver-blazorserver"
  location              = azurerm_resource_group.rg.location
  resource_group_name   = azurerm_resource_group.rg.name
  service_plan_id       = azurerm_service_plan.appserviceplan.id
  https_only            = true
  site_config {
    application_stack {
      dotnet_version = "6.0"
    }
    minimum_tls_version  = "1.2"
  }
}
resource "azurerm_linux_web_app" "apihost" {
  name                  = "apihost-blazorserver"
  location              = azurerm_resource_group.rg.location
  resource_group_name   = azurerm_resource_group.rg.name
  service_plan_id       = azurerm_service_plan.appserviceplan.id
  https_only            = true
  site_config {
    application_stack {
      dotnet_version = "6.0"
    }
    minimum_tls_version  = "1.2"
  }
}
resource "azurerm_linux_web_app" "webapp" {
  name                  = "webapp-blazorserver"
  location              = azurerm_resource_group.rg.location
  resource_group_name   = azurerm_resource_group.rg.name
  service_plan_id       = azurerm_service_plan.appserviceplan.id
  https_only            = true
  site_config {
    application_stack {
      dotnet_version = "6.0"
    }
    minimum_tls_version  = "1.2"
  }
}
```
    
    {{ else }}

```terraform
# Configure the Azure provider
terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.0.0"
    }
  }
  required_version = ">= 0.14.9"
}
provider "azurerm" {
  features {}
}

# Create the resource group
resource "azurerm_resource_group" "rg" {
  name     = "blazorserver-app-tier-rg"
  location = "westeurope"
}

# Create the Linux App Service Plan
resource "azurerm_service_plan" "appserviceplan" {
  name                = "blazorserver-app-tier-plan"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  os_type             = "Linux"
  sku_name            = "B3"
}

# Create the web app, pass in the App Service Plan ID
resource "azurerm_linux_web_app" "authserver" {
  name                  = "authserver-blazorserver"
  location              = azurerm_resource_group.rg.location
  resource_group_name   = azurerm_resource_group.rg.name
  service_plan_id       = azurerm_service_plan.appserviceplan.id
  https_only            = true
  site_config {
    application_stack {
      dotnet_version = "6.0"
    }
    minimum_tls_version  = "1.2"
  }
  app_settings = {
    "Redis__Configuration" = azurerm_redis_cache.redis.primary_connection_string
  }
}
resource "azurerm_linux_web_app" "apihost" {
  name                  = "apihost-blazorserver"
  location              = azurerm_resource_group.rg.location
  resource_group_name   = azurerm_resource_group.rg.name
  service_plan_id       = azurerm_service_plan.appserviceplan.id
  https_only            = true
  site_config {
    application_stack {
      dotnet_version = "6.0"
    }
    minimum_tls_version  = "1.2"
  }
  app_settings = {
    "Redis__Configuration" = azurerm_redis_cache.redis.primary_connection_string
  }
}
resource "azurerm_linux_web_app" "webapp" {
  name                  = "webapp-blazorserver"
  location              = azurerm_resource_group.rg.location
  resource_group_name   = azurerm_resource_group.rg.name
  service_plan_id       = azurerm_service_plan.appserviceplan.id
  https_only            = true
  site_config {
    application_stack {
      dotnet_version = "6.0"
    }
    minimum_tls_version  = "1.2"
  }
  app_settings = {
    "Redis__Configuration" = azurerm_redis_cache.redis.primary_connection_string
  }
}

resource "azurerm_redis_cache" "redis" {
  name                = "redis-blazorserver"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  capacity            = 0
  family              = "C"
  sku_name            = "Basic"
  enable_non_ssl_port = false
  minimum_tls_version = "1.2"

  redis_configuration {
    maxmemory_reserved = 2
    maxmemory_delta    = 2
    maxmemory_policy   = "volatile-lru"
  }
}
```

    {{end}}

{{ else if UI == "MVC" }}

    {{ if Tiered == "No" }}

```terraform
# Configure the Azure provider
terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.0.0"
    }
  }
  required_version = ">= 0.14.9"
}
provider "azurerm" {
  features {}
}

# Create the resource group
resource "azurerm_resource_group" "rg" {
  name     = "demo-abp-web-app"
  location = "westeurope"
}

# Create the Linux App Service Plan
resource "azurerm_service_plan" "appserviceplan" {
  name                = "demo-abp-web-app-plan"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  os_type             = "Linux"
  sku_name            = "B3"
}

# Create the web app, pass in the App Service Plan ID
resource "azurerm_linux_web_app" "webapp" {
  name                  = "demo-abp-web-app"
  location              = azurerm_resource_group.rg.location
  resource_group_name   = azurerm_resource_group.rg.name
  service_plan_id       = azurerm_service_plan.appserviceplan.id
  https_only            = true
  site_config {
    application_stack {
      dotnet_version = "6.0"
    }
    minimum_tls_version  = "1.2"
  }
}

output "webappurl" {
  
  value = "${azurerm_linux_web_app.webapp.name}.azurewebsites.net"
}
```

    {{ else }}

```terraform
# Configure the Azure provider
terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.0.0"
    }
  }
  required_version = ">= 0.14.9"
}
provider "azurerm" {
  features {}
}

# Create the resource group
resource "azurerm_resource_group" "rg" {
  name     = "demo-abp-web-app-tier-rg"
  location = "westeurope"
}

# Create the Linux App Service Plan
resource "azurerm_service_plan" "appserviceplan" {
  name                = "demo-abp-web-app-tier-plan"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  os_type             = "Linux"
  sku_name            = "B3"
}

# Create the web app, pass in the App Service Plan ID
resource "azurerm_linux_web_app" "authserver" {
  name                  = "authserver-prodemo"
  location              = azurerm_resource_group.rg.location
  resource_group_name   = azurerm_resource_group.rg.name
  service_plan_id       = azurerm_service_plan.appserviceplan.id
  https_only            = true
  site_config {
    application_stack {
      dotnet_version = "6.0"
    }
    minimum_tls_version  = "1.2"
  }
  app_settings = {
    "Redis__Configuration" = azurerm_redis_cache.redis.primary_connection_string
  }
}
resource "azurerm_linux_web_app" "apihost" {
  name                  = "apihost-prodemo"
  location              = azurerm_resource_group.rg.location
  resource_group_name   = azurerm_resource_group.rg.name
  service_plan_id       = azurerm_service_plan.appserviceplan.id
  https_only            = true
  site_config {
    application_stack {
      dotnet_version = "6.0"
    }
    minimum_tls_version  = "1.2"
  }
  app_settings = {
    "Redis__Configuration" = azurerm_redis_cache.redis.primary_connection_string
  }
}
resource "azurerm_linux_web_app" "webapp" {
  name                  = "webapp-prodemo"
  location              = azurerm_resource_group.rg.location
  resource_group_name   = azurerm_resource_group.rg.name
  service_plan_id       = azurerm_service_plan.appserviceplan.id
  https_only            = true
  site_config {
    application_stack {
      dotnet_version = "6.0"
    }
    minimum_tls_version  = "1.2"
  }
  app_settings = {
    "Redis__Configuration" = azurerm_redis_cache.redis.primary_connection_string
  }
}

resource "azurerm_redis_cache" "redis" {
  name                = "redis-prodemo"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  capacity            = 0
  family              = "C"
  sku_name            = "Basic"
  enable_non_ssl_port = false
  minimum_tls_version = "1.2"

  redis_configuration {
    maxmemory_reserved = 2
    maxmemory_delta    = 2
    maxmemory_policy   = "volatile-lru"
  }
}

output "authserver" {
  
  value = "${azurerm_linux_web_app.authserver.name}.azurewebsites.net"
}

output "apihost" {
  
  value = "${azurerm_linux_web_app.apihost.name}.azurewebsites.net"
}

output "webapp" {
  
  value = "${azurerm_linux_web_app.webapp.name}.azurewebsites.net"
}

output "redis_hostname" {
  value = azurerm_redis_cache.redis.hostname
  description = "The hostname for the Redis instance."
}
```

    {{end}}

{{end}}


3. Run `terraform init` to initialize the directory.

4. Run `terraform plan` to see the execution plan.

5. Run `terraform apply` to apply the changes. Write `yes` when prompted to confirm the deployment.

6. Wait for the deployment to complete.

7. Navigate to the web app URL to see the deployed application.

> You can also see the web app URL in the output of the `terraform apply` command.

> You have to change the **dotnet version** of the runtime stack according to your application. For example, if you are using .NET 7, you should change `dotnet_version = "6.0"` to `dotnet_version = "7.0"`.

![Azure Web App](../../../images/azure-deploy-runtime-stack.png)

## Destroying the Terraform Configuration

1. Run `terraform destroy` to destroy the created resources.

2. Type `yes` when prompted to confirm the destruction.

