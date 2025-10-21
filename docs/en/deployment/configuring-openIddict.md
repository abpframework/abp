```json
//[doc-seo]
{
    "Description": "Learn how to configure OpenIddict in your AuthServer project for both Development and Production environments with this comprehensive guide."
}
```

# Configuring OpenIddict

This document introduces how to configure `OpenIddict` in the `AuthServer` project.

There are different configurations in the `AuthServer` project for the `Development` and `Production` environments.

> If your solution does not include a project named `.AuthServer`, it means that you might have another project that depends on `AbpAccountPublicWebOpenIddictModule`. The project name can be `MyProject`, `MyProject.Web`, or `MyProject.HttpApi.Host`. They are both `Authentication Server` projects in that context.

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

`AddDevelopmentEncryptionAndSigningCertificate` is enabled by default on the development environment. It registers (and also generates, if necessary, a user-specific development encryption/development signing certificate. This is a certificate used for signing and encrypting the tokens and for **development environment only**.

`AddDevelopmentEncryptionAndSigningCertificate` cannot be used in applications deployed on IIS or Azure App Service: trying to use them on IIS or Azure App Service will result in an exception being thrown at runtime (unless the application pool is configured to [load a user profile](https://learn.microsoft.com/en-us/iis/manage/configuring-security/application-pool-identities#user-profile)). 

To avoid that, consider creating self-signed certificates and storing them in the X.509 certificates storage of the host machine(s). This is the way we do it in the production environment.

## Production Environment

`AddDevelopmentEncryptionAndSigningCertificate` is disabled in production environment. Signing and encryption of certificates is done using `openiddict.pfx` file in production environment.

You can use the `dotnet dev-certs https -v -ep openiddict.pfx -p 00000000-0000-0000-0000-000000000000` command to generate the `openiddict.pfx` certificate.

> `openiddict.pfx` is just an example of a filename. You can use any filename for the pfx file.

> `00000000-0000-0000-0000-000000000000` is the password of the certificate, you can change it to any password you want.

>  Also, please remember to copy `openiddict.pfx` to the [Content Root Folder](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.hosting.ihostingenvironment.contentrootpath?view=aspnetcore-7.0) of the `AuthServer` website.

> It is recommended to use **two** RSA certificates, distinct from the certificate(s) used for HTTPS: one for encryption, one for signing.

> If you encounter a deployment error on IIS that says **File not found** even though the file exists, it is recommended to set the application pool’s advanced settings **Load User Profile** to **True** to resolve the issue.

> The `X509KeyStorageFlags.MachineKeySet` and `X509KeyStorageFlags.EphemeralKeySet` flags can be set in the `AddProductionEncryptionAndSigningCertificate` method for IIS deployments. For example:

```csharp
serverBuilder.AddProductionEncryptionAndSigningCertificate(
   "openiddict.pfx",
   "your-password",
   X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.EphemeralKeySet);
```

For more information, please refer to: https://documentation.openiddict.com/configuration/encryption-and-signing-credentials.html#registering-a-certificate-recommended-for-production-ready-scenarios
