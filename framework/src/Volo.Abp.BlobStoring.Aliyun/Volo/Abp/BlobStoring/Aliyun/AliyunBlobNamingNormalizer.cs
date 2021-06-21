using System.Globalization;
using System.Text.RegularExpressions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;

namespace Volo.Abp.BlobStoring.Aliyun
{
    public class AliyunBlobNamingNormalizer : IBlobNamingNormalizer, ITransientDependency
    {
        /// <summary>
        /// Container names can contain only letters, numbers, and the dash (-) character
        /// they can't start or end with the dash (-) character
        /// Container names must be from 3 through 63 characters long
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

                // Container names can contain only letters, numbers, and the dash (-) character.
                containerName = Regex.Replace(containerName, "[^a-z0-9-]", string.Empty);

                // Every dash (-) character must be immediately preceded and followed by a letter or number;
                // consecutive dashes are not permitted in container names.
                // Container names must start or end with a letter or number
                containerName = Regex.Replace(containerName, "-{2,}", "-");
                containerName = Regex.Replace(containerName, "^-", string.Empty);
                containerName = Regex.Replace(containerName, "-$", string.Empty);

                // Container names must be from 3 through 63 characters long.
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

        public virtual string NormalizeBlobName(string blobName)
        {
            return blobName;
        }
    }
}
