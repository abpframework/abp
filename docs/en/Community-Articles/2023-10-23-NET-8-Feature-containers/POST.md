# New Containers feature with NET 8.0

This article will show you the new feature of containers with NET 8.0.

## Non-root user

The `Non-root user` feature on net 8 is a security measure that allows users to have limited access to the system without having full administrative privileges. Hosting containers as `non-root` aligns with the principle of least privilege. 
It’s free security provided by the operating system. If you run your app as root, your app process can do anything in the container, like modify files, install packages, or run arbitrary executables. 
That’s a concern if your app is ever attacked. If you run your app as non-root, your app process cannot do much, greatly limiting what a bad actor could accomplish.

## Default ASP.NET Core port changed from 80 to 8080

In .NET 8, there has been a change in the default port used by ASP.NET Core applications. Previously, the default port assigned to ASP.NET Core applications was `80`. However, starting from .NET 8, the default port has been changed to `8080`.
This change was made to avoid conflicts with other applications and services that commonly use port 80, such as web servers like IIS or Apache. By using port 8080 as the default, there is less potential for clashes and easier deployment of ASP.NET Core applications alongside other services.

It's important to note that this change only affects the default port used when an ASP.NET Core application is run without explicitly specifying a port. 

If you want your application to continue using port 80, you can still specify it during the application launch or configure it in the application settings.

* Recommended: Explicitly set the `ASPNETCORE_HTTP_PORTS`, `ASPNETCORE_HTTPS_PORTS`, and `ASPNETCORE_URLS` environment variables to the desired port. Example: `docker run --rm -it -p 9999:80 -e ASPNETCORE_HTTP_PORTS=80 <my-app>`
* Update existing commands and configuration that rely on the expected default port of port 80 to reference port 8080 instead. Example: `docker run --rm -it -p 9999:8080 <my-app>`

> The `dockerfile` of ABP templates has been updated to use port `80`.

## References

- [Secure your .NET cloud apps with rootless Linux Containers](https://devblogs.microsoft.com/dotnet/securing-containers-with-rootless/)
- [Containers breaking changes](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-8#containers)
- [ASP.NET Core apps use port 8080 by default](https://learn.microsoft.com/en-us/dotnet/core/compatibility/8.0#containers)
- [Docker images for ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/docker/building-net-docker-images?view=aspnetcore-8.0)
