using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Extensions.DependencyInjection;

public static class OpenIddictServerBuilderExtensions
{
    public static OpenIddictServerBuilder AddProductionEncryptionAndSigningCertificate(this OpenIddictServerBuilder builder, string fileName, string passPhrase, X509KeyStorageFlags? flag = null)
    {
        if (!File.Exists(fileName))
        {
            throw new FileNotFoundException($"Signing Certificate couldn't found: {fileName}");
        }

        var certificate = flag != null
            ? new X509Certificate2(fileName, passPhrase, flag.Value)
            : new X509Certificate2(fileName, passPhrase);

        builder.AddSigningCertificate(certificate);
        builder.AddEncryptionCertificate(certificate);
        return builder;
    }
}
