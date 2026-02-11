```json
//[doc-seo]
{
    "Description": "Learn how to handle common issues with reverse proxies in ASP.NET Core using forwarded headers middleware for your ABP applications."
}
```

# Forwarded Headers

Reverse proxies and load balancers play a crucial role in modern web application architectures. When an application is deployed behind these proxies and load balancers, several specific issues can arise. This document will discuss these issues in detail, explain how ASP.NET Core's forwarded headers middleware can address them, and provide a code example for configuring forwarded headers in an ABP application.

## Possible problem in a Reverse Proxy Environment

When requests pass through a reverse proxy or load balancer, the following common issues can occur:

### 1. Loss of Original Request Information

A reverse proxy or load balancer typically modifies the original HTTP request headers. For example, the proxy may replace the client's `X-Forwarded-For` header, or the `Host` header might be set to the proxy's address. This can result in the backend application being unable to directly access the client's IP address, the true hostname, and the protocol used.

### 2. HTTPS vs HTTP Protocol Confusion

When a request is forwarded by a proxy server, it is often upgraded to HTTPS to ensure secure transmission. The proxy server will send a header like `X-Forwarded-Proto` to indicate whether the original request was HTTP or HTTPS. If the backend application does not correctly handle this header, it may generate URLs with the wrong protocol.

### 3. Path Handling Issues

Since load balancers and proxies might modify or map the request URL paths differently, the backend application could encounter path inconsistencies. For example, a reverse proxy might forward a request from `/api` to `/myapp/api`. If the backend application is not correctly configured, path parsing errors may occur.

### 4. IP Address and Security

Reverse proxies might replace the original client IP address with their own, which can affect logging, authentication, and access control mechanisms. To retrieve the actual client IP address, the `X-Forwarded-For` header must be correctly parsed and trusted.

### 5. Load Balancer Impact

Load balancers might distribute requests to different backend servers using different algorithms. This can create session affinity problems. If session data is stored on a single server and the load balancer directs subsequent requests to different servers, session loss or inconsistency may occur.

## Forwarded Headers Middleware in ABP web application

To resolve the above issues, ASP.NET Core provides a built-in middleware, `ForwardedHeadersMiddleware`, which processes the headers forwarded by reverse proxies. This middleware helps the application recover the correct original request information, such as the client’s IP address, protocol, and host.

### Configuring `ForwardedHeadersMiddleware`

ASP.NET Core’s `ForwardedHeadersMiddleware` supports several HTTP headers:

- **X-Forwarded-For**: Contains the original client’s IP address.
- **X-Forwarded-Proto**: Indicates whether the original request was HTTP or HTTPS.
- **X-Forwarded-Host**: Contains the original host requested by the client.
- **X-Forwarded-Port**: Indicates the original port of the request.

To configure this middleware:

1. In the `ConfigureServices` method of your module, configure the `ForwardedHeadersOptions`:

```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
	context.Services.Configure<ForwardedHeadersOptions>(options =>
	{
		options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
	});
}
```

2. In the `OnApplicationInitialization` method of your module, add the middleware:

> Forwarded Headers Middleware should run before other middleware. This ordering ensures that the middleware relying on forwarded headers information can consume the header values for processing. Forwarded Headers Middleware can run after diagnostics and error handling, but it must be run before calling UseHsts:

```csharp
public override void OnApplicationInitialization(ApplicationInitializationContext context)
{
	var app = context.GetApplicationBuilder();
	var env = context.GetEnvironment();

	if (env.IsDevelopment())
	{
		app.UseDeveloperExceptionPage();
		app.UseForwardedHeaders();
	}
	else
	{
		app.UseErrorPage();
		app.UseForwardedHeaders();
		app.UseHsts();
	}

	// Other middleware configurations...
}
```

## References

- [ASP.NET Core Proxy and Load Balancer Configuration](https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-9.0)
