# Configuring OpenIddict

This document introduces how to configure `OpenIddict` in the `AuthServer` project.

There are different configurations in the `AuthServer` project for the `Development` and `Production` environments.

````csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
    var hostingEnvironment = context.Services.GetHostingEnvironment();

    if (!hostingEnvironment.IsDevelopment())
    {
       PreConfigure<AbpOpenIddictAspNetCoreOptions>(options =>
       {
          options.AddDevelopmentEncryptionAndSigningCertificate = false;
       });

       PreConfigure<OpenIddictServerBuilder>(serverBuilder =>
       {
          serverBuilder.AddProductionEncryptionAndSigningCertificate("openiddict.pfx", "00000000-0000-0000-0000-000000000000");
       });
    }
}
````

## Development Environment

We've enabled `AddDevelopmentEncryptionAndSigningCertificate` by default on the development environment. It registers (and generates if necessary) a user-specific development encryption/development signing certificate. This is a certificate used for signing and encrypting the tokens and for **development environment only**.

`AddDevelopmentEncryptionAndSigningCertificate` cannot be used in applications deployed on IIS or Azure App Service: trying to use them on IIS or Azure App Service will result in an exception being thrown at runtime (unless the application pool is configured to [load a user profile](https://learn.microsoft.com/en-us/iis/manage/configuring-security/application-pool-identities#user-profile)). 

To avoid that, consider creating self-signed certificates and storing them in the X.509 certificates storage of the host machine(s). This is the way we do it in the production environment.

## Production Environment

We've disabled `AddDevelopmentEncryptionAndSigningCertificate` in the production environment and tried to setup signing and encrypting certificates using `openiddict.pfx` file.

You can use the `dotnet dev-certs https -v -ep openiddict.pfx -p 00000000-0000-0000-0000-000000000000` command to generate the `authserver.pfx` certificate.

> `00000000-0000-0000-0000-000000000000` is the password of the certificate, you can change it to any password you want.

>  Also, please remember to copy `openiddict.pfx` to the [Content Root Folder](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.hosting.ihostingenvironment.contentrootpath?view=aspnetcore-7.0) of the `AuthServer` website.

> It is recommended to use **two** RSA certificates, distinct from the certificate(s) used for HTTPS: one for encryption, one for signing.

For more information, please refer to: https://documentation.openiddict.com/configuration/encryption-and-signing-credentials.html#registering-a-certificate-recommended-for-production-ready-scenarios
