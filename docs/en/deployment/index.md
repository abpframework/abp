```json
//[doc-seo]
{
    "Description": "Learn how to effectively deploy your ABP applications with essential guides on SSL, OpenIddict, and best practices for .NET environments."
}
```

# Deployment

Deploying an ABP application is not different than deploying any .NET or ASP.NET Core application. You can deploy it to a cloud provider (e.g., Azure, AWS, Google Could) or an on-premise server, IIS or any other web server. ABP's documentation doesn't contain much information on deployment. You can refer to your provider's documentation.

However, there are some topics that you should care about when you are deploying your applications. Most of them are general software deployment considerations, but you should understand how to handle them within your ABP based applications. We've prepared guides for this purpose and we suggest you to read these guides carefully before designing your deployment configuration.

## Guides

* [Configuring SSL certificate(HTTPS)](./ssl.md): Explains how to configure SSL certificate(HTTPS) for your application.
* [Configuring OpenIddict](./configuring-openIddict.md): Notes for some essential configurations for OpenIddict.
* [Configuring for Production](./configuring-production.md): Notes for some essential configurations for production environments.
* [Optimization for Production](./optimizing-production.md): Tips and suggestions for optimizing your application in production environments.
* [Deploying to a Clustered Environment](./clustered-environment.md): Explains how to configure your application when you want to run multiple instances of your application concurrently.
* [Deploying Distributed / Microservice Solutions](./distributed-microservice.md): Deployment notes for solutions consisting of multiple applications and/or services.
* [Forwarded Headers](./forwarded-headers): Explains how to configure the application to trust headers forwarded by reverse proxies and recover the original request information from the `X-Forwarded-For` and `X-Forwarded-Proto` headers.