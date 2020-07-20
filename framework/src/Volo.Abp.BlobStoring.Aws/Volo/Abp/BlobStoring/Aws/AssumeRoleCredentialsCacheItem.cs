using System;
using Volo.Abp.Caching;

namespace Volo.Abp.BlobStoring.Aws
{
    [Serializable]
    [CacheName("TemporaryCredentials")]
    public class TemporaryCredentialsCacheItem
    {
        public const string Key = "AwsTemporaryCredentialsCache";

        public string AccessKeyId { get; set; }

        public string SecretAccessKey { get; set; }

        public string SessionToken { get; set; }

        public TemporaryCredentialsCacheItem()
        {

        }

        public TemporaryCredentialsCacheItem(string accessKeyId,string secretAccessKey,string sessionToken)
        {
            AccessKeyId = accessKeyId;
            SecretAccessKey = secretAccessKey;
            SessionToken = sessionToken;
        }
    }
}
