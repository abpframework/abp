using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;

namespace Volo.Abp.BlobStoring.Minio;

public class MinioBlobNamingNormalizer : IBlobNamingNormalizer, ITransientDependency
{
    /// <summary>
    ///https://docs.aws.amazon.com/AmazonS3/latest/dev/BucketRestrictions.html
    /// </summary>
    public virtual string NormalizeContainerName(string containerName)
    {
        using (CultureHelper.Use(CultureInfo.InvariantCulture))
        {
            // All letters in a container name must be lowercase.
            containerName = containerName.ToLower();

            // Container names must be from 3 through 63 characters long.
            if (containerName.Length > 63)
            {
                containerName = containerName.Substring(0, 63);
            }

            // Bucket names can consist only of lowercase letters, numbers, dots (.), and hyphens (-).
            containerName = Regex.Replace(containerName, "[^a-z0-9-.]", string.Empty);

            // Bucket names must begin and end with a letter or number.
            // Bucket names must not be formatted as an IP address (for example, 192.168.5.4).
            // Bucket names can't start or end with hyphens adjacent to period
            // Bucket names can't start or end with dots adjacent to period
            containerName = Regex.Replace(containerName, "\\.{2,}", ".");
            containerName = Regex.Replace(containerName, "-\\.", string.Empty);
            containerName = Regex.Replace(containerName, "\\.-", string.Empty);
            containerName = Regex.Replace(containerName, "^-", string.Empty);
            containerName = Regex.Replace(containerName, "-$", string.Empty);
            containerName = Regex.Replace(containerName, "^\\.", string.Empty);
            containerName = Regex.Replace(containerName, "\\.$", string.Empty);
            containerName = Regex.Replace(containerName, "^(?:(?:^|\\.)(?:2(?:5[0-5]|[0-4]\\d)|1?\\d?\\d)){4}$", String.Empty);

            if (containerName.Length < 3)
            {
                var length = containerName.Length;
                for (var i = 0; i < 3 - length; i++)
                {
                    containerName += "0";
                }
            }

            return containerName;
        }
    }

    /// <summary>
    /// https://docs.aws.amazon.com/AmazonS3/latest/dev/UsingMetadata.html
    /// </summary>
    public virtual string NormalizeBlobName(string blobName)
    {
        return blobName;
    }
}
