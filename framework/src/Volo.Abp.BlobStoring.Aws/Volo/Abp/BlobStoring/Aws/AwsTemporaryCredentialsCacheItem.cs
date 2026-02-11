using System;

namespace Volo.Abp.BlobStoring.Aws;

[Serializable]
public class AwsTemporaryCredentialsCacheItem
{
    public string AccessKeyId { get; set; } = default!;

    public string SecretAccessKey { get; set; } = default!;

    public string SessionToken { get; set; } = default!;

    public AwsTemporaryCredentialsCacheItem()
    {

    }

    public AwsTemporaryCredentialsCacheItem(string accessKeyId, string secretAccessKey, string sessionToken)
    {
        AccessKeyId = accessKeyId;
        SecretAccessKey = secretAccessKey;
        SessionToken = sessionToken;
    }
}
