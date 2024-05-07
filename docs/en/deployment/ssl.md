# Configuring Configuring SSL certificate(HTTPS)

A website needs an SSL certificate in order to keep user data secure, verify ownership of the website, prevent attackers from creating a fake version of the site, and gain user trust.

This document introduces how to get and use SSL certificate(HTTPS) for your application.

## Get a SSL Certificate from a Certificate Authority

You can get a SSL certificate from a certificate authority (CA) such as [Let's Encrypt](https://letsencrypt.org/) or [Cloudflare](https://www.cloudflare.com/learning/ssl/what-is-an-ssl-certificate/) and so on.

Once you have a certificate, you need to configure your web server to use it. The following references show how to configure your web server to use a certificate.

* [Host ASP.NET Core on Linux with Apache: HTTPS configuration](https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-apache)
* [Host ASP.NET Core on Linux with Nginx: HTTPS configuration](https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-nginx)
* [How to Set Up SSL on IIS 7 or later](https://learn.microsoft.com/en-us/iis/manage/configuring-security/how-to-set-up-ssl-on-iis)

## Create a Self-Signed Certificate

You can create a self-signed certificate for testing purposes or internal use.

There is an article about [how to create a self-signed certificate](https://learn.microsoft.com/en-us/dotnet/core/additional-tools/self-signed-certificates-guide), If you are using IIS, you can use the following this document to [obtain a Certificate](https://learn.microsoft.com/en-us/iis/manage/configuring-security/how-to-set-up-ssl-on-iis#obtain-a-certificate)

## Common Problems

### The remote certificate is invalid because of errors in the certificate chain: UntrustedRoot

This error may occur when using IIS. You need to trust your certificate by `Manage computer certificates`.

## References

* [ABP IIS Deployment](https://docs.abp.io/en/commercial/latest/startup-templates/application/deployment-iis)
* [HTTPS in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/enforcing-ssl)
* [Let's Encrypt](https://letsencrypt.org/getting-started)
* [Cloudflare's Free SSL / TLS](https://www.cloudflare.com/application-services/products/ssl/)