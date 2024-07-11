# Microservice Solution: HTTP API Calls

````json
//[doc-nav]
{
  "Next": {
    "Name": "gRPC calls in the Microservice solution",
    "Path": "solution-templates/microservice/grpc-calls"
  }
}
````

> You must have an ABP Business or a higher license to be able to create a microservice solution.

The microservice solution template uses the [Integration Services](../../framework/api-development/integration-services.md) to enable synchronous communication between microservices. This allows microservices to communicate with each other over HTTP requests. You can also use the [Static](../../framework/api-development/static-csharp-clients.md) or [Dynamic](../../framework/api-development/dynamic-csharp-clients.md) C# clients to call the HTTP APIs of other microservices. However, the Integration Services is the recommended way to communicate between microservices because it allows you to hide your endpoints from the outside world and manage your services more easily.

## Integration Service Configurations

By default, the Integration Services configurations do not expose the endpoints. To enable communication between microservices, you need to configure the *Module* class of your microservice project. The following code shows how to configure the Integration Services in the Module class:

```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    ConfigureIntegrationServices();
}

private void ConfigureIntegrationServices()
{
    Configure<AbpAspNetCoreMvcOptions>(options =>
    {
        options.ExposeIntegrationServices = true;
    });
}
```

After exposing the Integration Services, you can call the HTTP APIs of other microservices from the related microservice projects. To do this, you need to use the *Http.Client* package for the respective microservice/module project. Next, you can set the endpoint URL of the related microservice in the appsettings.json file. The following code shows how to set the endpoint URL of the related microservice in the appsettings.json file:

```json
"RemoteServices": {
    "AbpIdentity": {
        "BaseUrl": "http://localhost:44377"
    }
}
```

For a real-world example, inspect the *Administration* microservice, which communicates via HTTP with the *Identity* microservice.

> When you enable the Integration Services, ensure that only the microservices within the cluster can access the endpoints. The existing gateway applications do not expose the Integration Service endpoints to the outside world.