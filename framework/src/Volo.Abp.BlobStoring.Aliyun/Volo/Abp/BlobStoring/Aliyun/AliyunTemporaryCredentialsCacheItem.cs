using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Caching;

namespace Volo.Abp.BlobStoring.Aliyun
{
    [Serializable]
    public class AliyunTemporaryCredentialsCacheItem
    {
        public string AccessKeyId { get; set; }

        public string AccessKeySecret { get; set; }

        public string SecurityToken { get; set; }

        public AliyunTemporaryCredentialsCacheItem()
        {

        }

        public AliyunTemporaryCredentialsCacheItem(string accessKeyId,string accessKeySecret,string securityToken)
        {
            AccessKeyId = accessKeyId;
            AccessKeySecret = accessKeySecret;
            SecurityToken = securityToken;
        }
    }
}
