using System;

namespace Volo.Abp.BlobStoring.Aws
{
    [Serializable]
    public class AwsTemporaryCredentialsCacheItem
    {
        public string AccessKeyId { get; set; }

        public string SecretAccessKey { get; set; }

        public string SessionToken { get; set; }

        public AwsTemporaryCredentialsCacheItem()
        {

        }

        public AwsTemporaryCredentialsCacheItem(string accessKeyId,string secretAccessKey,string sessionToken)
        {
            AccessKeyId = accessKeyId;
            SecretAccessKey = secretAccessKey;
            SessionToken = sessionToken;
        }
    }
}
