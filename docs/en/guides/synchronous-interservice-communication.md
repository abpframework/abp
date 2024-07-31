# Synchronous Communication between Microservices

````json
//[doc-nav]
{
  "Next": {
    "Name": "Asynchronous Communication",
    "Path": "guides/asynchronous-interservice-communication"
  }
}
````

> This documentation introduces guidance for inter-service communication between two microservices using synchronous HTTP communication.

The below sample demonstrates how to make a request to **ProductService** from **OrderService**. See [Add New Microservice Guide](add-microservice.md) to understand how to add a new microservice project (_OrderService_) to your solution.

## Client Credentials Flow

![Add to main solution](../images/cc-flow.png)

The [Client Credentials flow](https://docs.identityserver.io/en/latest/quickstarts/1_client_credentials.html) is a server to server flow and **there is no user authentication** involved in the process. Instead of the user, the client ID (service itself) as the subject is authenticated with a predefined secret. The resulting access token contains client information instead of user information.

## Updating OrderService Application Contracts

In order to make ProductService application services available for OrderService application, you need to add the project reference of **ProductService.Application.Contracts** to **OrderService.Application.Contracts** and then add the module dependency attribute to the **OrderServiceApplicationContractsModule** class. Here are the steps:

**Add csproj reference**:

Open **Acme.BookStore.OrderService.Application.Contracts.csproj** and add the following project reference

```xml
<ProjectReference Include="..\..\..\product\src\Acme.BookStore.ProductService.Application.Contracts\Acme.BookStore.ProductService.Application.Contracts.csproj" />
```

**Add DependsOn attribute**:

Open **OrderServiceApplicationContractsModule.cs** class and add the following module dependency

```csharp
typeof(ProductServiceApplicationContractsModule)
```

![Add order service module dependency into the Administration Service](../images/orderservice-module-added-productservice.png)

Now you can call Product Application Services from the OrderService application :

![Add order service module dependency into the Administration Service](../images/orderservice-using-product-services.png)

> Build the **OrderService.Application** project with `dotnet build` command if you are having problems with resolving the new namespaces.

Since there is no implementation of `IProductAppService` and `IProductPublicAppService` in **OrderService.Application**, you need to configure it to make remote HTTP calls. To achieve it, you need to add the project reference of **Acme.BookStore.ProductService.HttpApi.Client** project to **Acme.BookStore.OrderService.Application** and then add the module dependency to **OrderServiceApplicationModule**. Here are the steps:

**Add csproj reference**:

Open **Acme.BookStore.OrderService.Application.csproj** and add the following project reference

```xml
<ProjectReference Include="..\..\..\product\src\Acme.BookStore.ProductService.HttpApi.Client\Acme.BookStore.ProductService.HttpApi.Client.csproj" />
```

**Add DependsOn attribute**:

Open **OrderServiceApplicationModule.cs** class and add the following module dependency

```csharp
typeof(ProductServiceHttpApiClientModule)
```

![Add order service module dependency into the Administration Service](../images/orderservice-app-module-added-product-client.png)

Open the **appsettings.json** file in the **OrderService.HttpApi.Host** project and update **RemoteServices** section to route HTTP requests to the **internal gateway**. Then these requests will be redirected to the Product Service.

```json
"RemoteServices": {
  "Default": {
    "BaseUrl": "https://localhost:44302/",
    "UseCurrentAccessToken": "false"
  }
}
```

**appsettings.json** of **OrderService.HttpApi.Host** project should look like below:

![Add order service module dependency into the Administration Service](../images/orderservice-httpapihost-appsettings-added-remoteservice.png)

## Configuring Auto-Discovery Endpoint

To automate requesting access token and adding it as `bearer` to the request headers; add [Volo.Abp.Http.Client.IdentityModel](https://www.nuget.org/packages/Volo.Abp.Http.Client.IdentityModel/) NuGet package to the **OrderService.HttpApi.Host** project.

**Add package reference**:

Open **Acme.BookStore.OrderService.HttpApi.Host.csproj** and add the following line (update the version attribute according to your project!)

```xml
<PackageReference Include="Volo.Abp.Http.Client.IdentityModel" Version="4.4.2" />
```

**Add DependsOn attribute**:

Open **OrderServiceHttpApiHostModule.cs** class and add the following module dependency

```csharp
typeof(AbpHttpClientIdentityModelModule)
```

![Add order service module dependency into the Administration Service](../images/orderservice-app-module-added-product-client.png)

Update **IdentityClients** section in **appsettings.json** file of the **OrderService.HttpApi.Host** to configure Client Credential access token request with client secret to the **AuthServer** end point.

```json
  "IdentityClients": {
    "Default": {
      "GrantType": "client_credentials", 
      "ClientId": "BookStore_OrderService",
      "ClientSecret": "1q2w3e*",
      "Authority": "https://localhost:44322", 
      "Scope": "ProductService"
    }
  }
```

**OrderService.HttpApi.Host** appsettings should look like below:

![Add order service module dependency into the Administration Service](../images/orderservice-httpapihost-appsettings.png)

## IdentityServer Configuration

> You can also do the same functionality explained in this step by using IdentityServer Management UI. However it is a good practice to keep `IdentityServerDataSeeder` up to date.

To keep `IdentityServerDataSeeder` updated, you need to do the following steps in the **IdentityServerDataSeeder.cs** class. Note that there are 2 **IdentityServerDataSeeder.cs** classes in the solution:

1.  `Acme.BookStore.DbMigrator\IdentityServerDataSeeder.cs`
2.  `Acme.BookStore.IdentityService.HttpApi.Host\DbMigrations\IdentityServerDataSeeder.cs`.

**Add ProductService.Application.Contracts Reference for Permissions:**

> If you will use the DbMigrator application, you can skip this step since DbMigrator project already has this reference.

For `Acme.BookStore.IdentityService.HttpApi.Host\DbMigrations\IdentityServerDataSeeder.cs` add **ProductService.Application.Contracts** project reference as below:

```xml
<ProjectReference Include="..\..\..\product\src\Acme.BookStore.ProductService.Application.Contracts\Acme.BookStore.ProductService.Application.Contracts.csproj" />
```

**Create New Client:**

`OrderService` itself is a new client and you need to add the **OrderService** as a client under **CreateClientsAsync** as well. Open `IdentityServerDataSeeder.cs` and add the below line in

```csharp
await CreateClientAsync(
    name: "BookStore_OrderService",
    scopes: commonScopes.Union(new[]
    {
        "ProductService"
    }),
    grantTypes: new[] {"client_credentials"},
    secret: "1q2w3e*".Sha256(),
    permissions: new[] {ProductServicePermissions.Products.Default}
);
```

> **OrderService.HttpApi.Host** appsettings **IdentityClients** section data **must match** with the client creation data.

**ProductAppService** has permission authorization and will return `401` unauthorized exception when a request has been made. To overcome this issue, add the required permissions of the service you are making to.

![Add order service module dependency into the Administration Service](../images/productappservice-authorization.png)

Since OrderService will only use **GetList** method of the **ProductAppService**, we are adding `ProductServicePermissions.Products.Default` permission for this client.

Below you can see a screenshot of the final **IdentityServerDataSeeder.cs**, creating `OrderService` client:

![IdentityServerDataSeeder changes](../images/identityserver-orderservice-client-creation.png)

When the databases are seeded, **BookStore_Identity.IdentityServerClients** database should have a new client and **BookStore_Administration.AbpPermissionGrants** should be seeded with ProviderName **C**:

![IdentityServerDataSeeder changes](../images/added-orderservice-client-db-query.png)
