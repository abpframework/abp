# Configuring OpenIddict

This document introduces how to configure `OpenIddict` in the `AuthServer` project.

There are different configurations in the `AuthServer` project for `Development` and `Production` environment.

````csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
    var hostingEnvironment = context.Services.GetHostingEnvironment();

    // Development environment
    if (hostingEnvironment.IsDevelopment())
    {
        PreConfigure<AbpOpenIddictAspNetCoreOptions>(options =>
        {
            // This is default value, you can remove this line.
            options.AddDevelopmentEncryptionAndSigningCertificate = true;
        });
    }

    // Production or Staging environment
    if (!hostingEnvironment.IsDevelopment())
    {
        PreConfigure<AbpOpenIddictAspNetCoreOptions>(options =>
        {
            options.AddDevelopmentEncryptionAndSigningCertificate = false;
        });

        PreConfigure<OpenIddictServerBuilder>(builder =>
        {
            builder.AddSigningCertificate(GetSigningCertificate(hostingEnvironment));
            builder.AddEncryptionCertificate(GetSigningCertificate(hostingEnvironment));

            //...
        });
    }
}

private X509Certificate2 GetSigningCertificate(IWebHostEnvironment hostingEnv)
{
    return new X509Certificate2(Path.Combine(hostingEnv.ContentRootPath, "authserver.pfx"), "00000000-0000-0000-0000-000000000000");
}
````

## Development Environment

We've enabled `AddDevelopmentEncryptionAndSigningCertificate` by default on development environment, It registers (and generates if necessary) a user-specific development encryption/development signing certificate. This is a certificate used for signing and encrypting the tokens and for **development environment only**.

`AddDevelopmentEncryptionAndSigningCertificate` cannot be used in applications deployed on IIS or Azure App Service: trying to use them on IIS or Azure App Service will result in an exception being thrown at runtime (unless the application pool is configured to [load a user profile](https://learn.microsoft.com/en-us/iis/manage/configuring-security/application-pool-identities#user-profile)). 

To avoid that, consider creating self-signed certificates and storing them in the X.509 certificates storage of the host machine(s). This is the way we do it in production environment.

## Production Environment

We've disabled `AddDevelopmentEncryptionAndSigningCertificate` in production environment and tried to setup signing and encrypting certificates using `authserver.pfx`.

You can use the `dotnet dev-certs https -v -ep authserver.pfx -p 00000000-0000-0000-0000-000000000000` command to generate the `authserver.pfx` certificate.

> `00000000-0000-0000-0000-000000000000` is the password of the certificate, you can change it to any password you want.

>  Also, please remember to copy `authserver.pfx` to the [Content Root Folder](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.hosting.ihostingenvironment.contentrootpath?view=aspnetcore-7.0) of the `AuthServer` website.
