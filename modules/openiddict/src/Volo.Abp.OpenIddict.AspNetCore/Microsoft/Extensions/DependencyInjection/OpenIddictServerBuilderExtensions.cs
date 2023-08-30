using System.IO;
using System.Security.Cryptography.X509Certificates;
using Volo.Abp.OpenIddict;

namespace Microsoft.Extensions.DependencyInjection;

public static class OpenIddictServerBuilderExtensions
{
    public static OpenIddictServerBuilder AddProductionEncryptionAndSigningCertificate(this OpenIddictServerBuilder builder, string fileName, string passPhrase)
    {
        builder.Services.PreConfigure<AbpOpenIddictAspNetCoreOptions>(options =>
        {
            options.AddDevelopmentEncryptionAndSigningCertificate = false;
        });

        if (!File.Exists(fileName))
        {
            throw new FileNotFoundException($"Signing Certificate couldn't found: {fileName}");
        }

        var certificate = new X509Certificate2(fileName, passPhrase);
        builder.AddSigningCertificate(certificate);
        builder.AddEncryptionCertificate(certificate);
        return builder;
    }
}
