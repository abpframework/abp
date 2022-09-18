# Consuming gRPC Services from Blazor WebAssembly Application Using gRPC-Web

> **WARNING: I've demonstrated [Using gRPC with the ABP Framework](https://community.abp.io/posts/using-grpc-with-the-abp-framework-2dgaxzw3) in my latest post. If you haven't seen it, you should read it before this article, since this is a continuation of that article.**

In this second part, I will show how to consume the gRPC service from the Blazor WebAssembly application, using the gRPC-Web technology.

This will be a short article, based on Microsoft's [gRPC-Web in ASP.NET Core gRPC apps](https://learn.microsoft.com/en-us/aspnet/core/grpc/grpcweb) and [Code-first gRPC services and clients with .NET](https://learn.microsoft.com/en-us/aspnet/core/grpc/code-first) documents. For more information, I suggest to check these documents. Let's get start...

## Configuring the Server Side

First of all, the server-side should support gRPC-Web. Follow the steps below to enable it:

### Add Grpc.AspNetCore.Web Package

Add [Grpc.AspNetCore.Web](https://www.nuget.org/packages/Grpc.AspNetCore.Web) NuGet package to the `ProductManagement.HttpApi.Host` project.

### Add GrpcWeb Middleware

Add the following line just before the `app.UseConfiguredEndpoints(...)` line to add the GrpcWeb middleware to your ASP.NET Core request pipeline:

````csharp
app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
````

### Configure Cors

ABP's startup template already configures Cors when you create a new solution. However, we need to allow some extra headers in our Cors configuration.

Add the following line just after the `.WithAbpExposedHeaders()` line in the `OnApplicationInitialization` method of the `ProductManagementHttpApiHostModule` class:

````csharp
.WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding")
````

Finally, call `RequireCors` extension method just after the `MapGrpcService` calls:

````csharp
app.UseConfiguredEndpoints(endpoints =>
{
    endpoints
        .MapGrpcService<IProductAppService>()
        .RequireCors("__DefaultCorsPolicy"); // Configure Cors for the product service
});
````

`__DefaultCorsPolicy` may seem a magic string here. Let me explain it: ABP startup template configures the default Cors policy with the `context.Services.AddCors(...)` method (you can see it in the source code). If we define a named policy, we should use the same name here. However, when we don't specify, ASP.NET Core uses `__DefaultCorsPolicy` as the policy name by default. If you don't want to use the magic string, you can resolve the `IOptions<CorsOptions>` service and get the `DefaultPolicyName` from the `CorsOption` object.

Anyway, that's all on the server-side. We can work on he client now.

## Configuring the Client Side

`ProductManagement.Blazor` is the Blazor WebAssembly application in the solution I'd created in the [first article](https://community.abp.io/posts/using-grpc-with-the-abp-framework-2dgaxzw3). We will configure that project to be able to consume the server-side gRPC services from our Blazor application.

### Add Client-side Nuget Packages

Add [Grpc.Net.Client](https://www.nuget.org/packages/Grpc.Net.Client), [Grpc.Net.Client.Web](https://www.nuget.org/packages/Grpc.Net.Client.Web) and [protobuf-net.Grpc](https://www.nuget.org/packages/protobuf-net.Grpc) NuGet packages to the `ProductManagement.Blazor` project.

