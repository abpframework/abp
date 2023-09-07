using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Extensions.DependencyInjection;

public static class OpenIddictServerBuilderExtensions
{
    public static OpenIddictServerBuilder AddProductionEncryptionAndSigningCertificate(this OpenIddictServerBuilder builder, string fileName, string passPhrase)
    {
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
