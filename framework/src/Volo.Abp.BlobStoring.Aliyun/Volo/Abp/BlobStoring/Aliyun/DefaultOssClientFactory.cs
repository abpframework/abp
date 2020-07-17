using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Auth.Sts;
using Aliyun.Acs.Core.Http;
using Aliyun.Acs.Core.Profile;
using Aliyun.OSS;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlobStoring.Aliyun
{
    /// <summary>
    /// Sub-account access to OSS or STS temporary authorization to access OSS
    /// </summary>
    public class DefaultOssClientFactory : IOssClientFactory, ITransientDependency
    {
        protected IDistributedCache<AssumeRoleCredentialsCacheItem> Cache { get; }
        public DefaultOssClientFactory(
            IDistributedCache<AssumeRoleCredentialsCacheItem> cache)
        {
            Cache = cache;
        }

        public virtual IOss Create(AliyunBlobProviderConfiguration aliyunConfig)
        {
            //Sub-account
            if (aliyunConfig.DurationSeconds <= 0)
            {
                return new OssClient(aliyunConfig.Endpoint, aliyunConfig.AccessKeyId, aliyunConfig.AccessKeySecret);
            }
            else
            {
                //STS temporary authorization to access OSS
                var key = aliyunConfig.ToKeyString();
                var cacheItem = Cache.Get(key);
                if (cacheItem == null)
                {
                    IClientProfile profile = DefaultProfile.GetProfile(
                    aliyunConfig.RegionId,
                    aliyunConfig.AccessKeyId,
                    aliyunConfig.AccessKeySecret);
                    DefaultAcsClient client = new DefaultAcsClient(profile);
                    AssumeRoleRequest request = new AssumeRoleRequest
                    {
                        AcceptFormat = FormatType.JSON,
                        //eg:acs:ram::$accountID:role/$roleName
                        RoleArn = aliyunConfig.RoleArn,
                        RoleSessionName = aliyunConfig.RoleSessionName,
                        //Set the validity period of the temporary access credential, the unit is s, the minimum is 900, and the maximum is 3600. default 3600
                        DurationSeconds = aliyunConfig.DurationSeconds,
                        //Set additional permission policy of Token; when acquiring Token, further reduce the permission of Token by setting an additional permission policy
                        Policy = aliyunConfig.Policy.IsNullOrEmpty() ? null : aliyunConfig.Policy,
                    };
                    var response = client.GetAcsResponse(request);
                    cacheItem = new AssumeRoleCredentialsCacheItem(response.Credentials.AccessKeyId, response.Credentials.AccessKeySecret, response.Credentials.SecurityToken);
                    Cache.Set(key, cacheItem, new DistributedCacheEntryOptions()
                    {
                        //Subtract 10 seconds of network request time.
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(aliyunConfig.DurationSeconds - 10)
                    });
                }
                return new OssClient(aliyunConfig.Endpoint, cacheItem.AccessKeyId, cacheItem.AccessKeySecret, cacheItem.SecurityToken); 
            }
        }
    }
}
