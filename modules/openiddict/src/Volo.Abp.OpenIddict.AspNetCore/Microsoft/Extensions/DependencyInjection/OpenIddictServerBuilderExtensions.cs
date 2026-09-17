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
            ? X509CertificateLoader.LoadPkcs12FromFile(fileName, passPhrase, flag.Value)
            : X509CertificateLoader.LoadPkcs12FromFile(fileName, passPhrase);

        builder.AddSigningCertificate(certificate);
        builder.AddEncryptionCertificate(certificate);
        return builder;
    }
}
