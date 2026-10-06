using System;
using System.IO;
using System.Linq;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.BlobStoring.FileSystem;

public class DefaultBlobFilePathCalculator : IBlobFilePathCalculator, ITransientDependency
{
    protected ICurrentTenant CurrentTenant { get; }

    public DefaultBlobFilePathCalculator(ICurrentTenant currentTenant)
    {
        CurrentTenant = currentTenant;
    }

    public virtual string Calculate(BlobProviderArgs args)
    {
        var fileSystemConfiguration = args.Configuration.GetFileSystemConfiguration();
        var blobPath = Path.GetFullPath(fileSystemConfiguration.BasePath);

        if (CurrentTenant.Id == null)
        {
            blobPath = Path.Combine(blobPath, "host");
        }
        else
        {
            blobPath = Path.Combine(blobPath, "tenants", CurrentTenant.Id.Value.ToString("D"));
        }

        if (fileSystemConfiguration.AppendContainerNameToBasePath)
        {
            blobPath = CombineRelativePath(blobPath, args.ContainerName, nameof(args.ContainerName));
        }

        return CombineRelativePath(blobPath, args.BlobName, nameof(args.BlobName));
    }

    protected virtual string CombineRelativePath(string rootPath, string relativePath, string parameterName)
    {
        if (Path.IsPathRooted(relativePath))
        {
            throw new ArgumentException($"The {parameterName} must be a relative path.", parameterName);
        }

        var segments = relativePath.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
        if (segments.Any(segment => segment == "." || segment == ".."))
        {
            throw new ArgumentException($"The {parameterName} must not contain '.' or '..' segments.", parameterName);
        }

        var fullRootPath = Path.GetFullPath(rootPath).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        var fullPath = Path.GetFullPath(Path.Combine(fullRootPath, relativePath));

        if (!fullPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).StartsWith(fullRootPath + Path.DirectorySeparatorChar, StringComparison.Ordinal))
        {
            throw new ArgumentException($"The {parameterName} must point to a location inside the storage directory.", parameterName);
        }

        return fullPath;
    }
}
