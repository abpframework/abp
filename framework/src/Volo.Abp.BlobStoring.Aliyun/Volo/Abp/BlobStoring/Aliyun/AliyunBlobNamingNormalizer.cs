﻿using System.Text.RegularExpressions;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlobStoring.Aliyun
{
    public class AliyunBlobNamingNormalizer : IBlobNamingNormalizer, ITransientDependency
    {
        /// <summary>
        /// 只允许小写字母、数字、短横线（-），且不能以短横线开头或结尾
        /// Container names can contain only letters, numbers, and the dash (-) character 
        /// can't start or end with the dash (-) character 
        /// 3~63 个字符
        /// Container names must be from 3 through 63 characters long
        /// </summary>
        public virtual string NormalizeContainerName(string containerName)
        {
            // All letters in a container name must be lowercase.
            containerName = containerName.ToLower();

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

            if (containerName.Length > 63)
            {
                containerName = containerName.Substring(0, 63);
            }

            // can't start or end with the dash (-) character 
            containerName = containerName.Trim('-');

            return containerName;
        }

        public virtual string NormalizeBlobName(string blobName)
        {
            return blobName;
        }
    }
}
