using System;
using System.Globalization;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;

namespace Volo.Abp.BlobStoring.FileSystem;

public class FileSystemBlobNamingNormalizer : IBlobNamingNormalizer, ITransientDependency
{
    private readonly IOSPlatformProvider _iosPlatformProvider;

    public FileSystemBlobNamingNormalizer(IOSPlatformProvider iosPlatformProvider)
    {
        _iosPlatformProvider = iosPlatformProvider;
    }

    public virtual string NormalizeContainerName(string containerName)
    {
        return Normalize(containerName);
    }

    public virtual string NormalizeBlobName(string blobName)
    {
        return Normalize(blobName);
    }

    protected virtual string Normalize(string fileName)
    {
        using (CultureHelper.Use(CultureInfo.InvariantCulture))
        {
            var os = _iosPlatformProvider.GetCurrentOSPlatform();
            if (os == OSPlatform.Windows)
            {
                // A filename cannot contain any of the following characters: \ / : * ? " < > |
                // In order to support the directory included in the blob name, remove / and \
                fileName = Regex.Replace(fileName, "[:\\*\\?\"<>\\|]", string.Empty);
            }

            return fileName;
        }
    }
}
