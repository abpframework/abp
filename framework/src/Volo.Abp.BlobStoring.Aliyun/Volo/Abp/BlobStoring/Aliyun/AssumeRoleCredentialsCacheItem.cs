using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Caching;

namespace Volo.Abp.BlobStoring.Aliyun
{
    [Serializable]
    [CacheName("AssumeRoleCredentials")]
    public class AssumeRoleCredentialsCacheItem
    {
        public string AccessKeyId { get; set; }

        public string AccessKeySecret { get; set; }

        public string SecurityToken { get; set; }

        public AssumeRoleCredentialsCacheItem()
        {

        }

        public AssumeRoleCredentialsCacheItem(string accessKeyId,string accessKeySecret,string securityToken)
        {
            AccessKeyId = accessKeyId;
            AccessKeySecret = accessKeySecret;
            SecurityToken = securityToken;
        }
    }
}
