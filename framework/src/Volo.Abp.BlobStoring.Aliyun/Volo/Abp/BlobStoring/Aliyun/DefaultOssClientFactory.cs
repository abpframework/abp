using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Auth.Sts;
using Aliyun.Acs.Core.Http;
using Aliyun.Acs.Core.Profile;
using Aliyun.OSS;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Encryption;
using static Aliyun.Acs.Core.Auth.Sts.AssumeRoleResponse;

namespace Volo.Abp.BlobStoring.Aliyun
{
    /// <summary>
    /// Sub-account access to OSS or STS temporary authorization to access OSS
    /// </summary>
    public class DefaultOssClientFactory : IOssClientFactory, ITransientDependency
    {
        protected IDistributedCache<AliyunTemporaryCredentialsCacheItem> Cache { get; }

        protected IStringEncryptionService StringEncryptionService { get; }

        public DefaultOssClientFactory(
            IDistributedCache<AliyunTemporaryCredentialsCacheItem> cache,
            IStringEncryptionService stringEncryptionService)
        {
            Cache = cache;
            StringEncryptionService = stringEncryptionService;
        }

        public virtual IOss Create(AliyunBlobProviderConfiguration configuration)
        {
            Check.NotNullOrWhiteSpace(configuration.AccessKeyId, nameof(configuration.AccessKeyId));
            Check.NotNullOrWhiteSpace(configuration.AccessKeySecret, nameof(configuration.AccessKeySecret));
            Check.NotNullOrWhiteSpace(configuration.Endpoint, nameof(configuration.Endpoint));
            if (configuration.UseSecurityTokenService)
            {
                //STS temporary authorization to access OSS
                return GetSecurityTokenClient(configuration);
            }
            //Sub-account
            return new OssClient(configuration.Endpoint, configuration.AccessKeyId, configuration.AccessKeySecret);
        }

        protected virtual IOss GetSecurityTokenClient(AliyunBlobProviderConfiguration configuration)
        {
            Check.NotNullOrWhiteSpace(configuration.RoleArn, nameof(configuration.RoleArn));
            Check.NotNullOrWhiteSpace(configuration.RoleSessionName, nameof(configuration.RoleSessionName));
            var cacheItem = Cache.Get(configuration.TemporaryCredentialsCacheKey);
            if (cacheItem == null)
            {
                IClientProfile profile = DefaultProfile.GetProfile(
                configuration.RegionId,
                configuration.AccessKeyId,
                configuration.AccessKeySecret);
                DefaultAcsClient client = new DefaultAcsClient(profile);
                AssumeRoleRequest request = new AssumeRoleRequest
                {
                    AcceptFormat = FormatType.JSON,
                    //eg:acs:ram::$accountID:role/$roleName
                    RoleArn = configuration.RoleArn,
                    RoleSessionName = configuration.RoleSessionName,
                    //Set the validity period of the temporary access credential, the unit is s, the minimum is 900, and the maximum is 3600. default 3600
                    DurationSeconds = configuration.DurationSeconds,
                    //Set additional permission policy of Token; when acquiring Token, further reduce the permission of Token by setting an additional permission policy
                    Policy = configuration.Policy.IsNullOrEmpty() ? null : configuration.Policy,
                };
                var response = client.GetAcsResponse(request);
                cacheItem = SetTemporaryCredentialsCache(configuration, response.Credentials);
            }
            return new OssClient(
                configuration.Endpoint,
                StringEncryptionService.Decrypt(cacheItem.AccessKeyId),
                StringEncryptionService.Decrypt(cacheItem.AccessKeySecret),
                StringEncryptionService.Decrypt(cacheItem.SecurityToken));
        }

        private AliyunTemporaryCredentialsCacheItem SetTemporaryCredentialsCache(
            AliyunBlobProviderConfiguration configuration,
            AssumeRole_Credentials credentials)
        {
            var temporaryCredentialsCache = new AliyunTemporaryCredentialsCacheItem(
                StringEncryptionService.Encrypt(credentials.AccessKeyId),
                StringEncryptionService.Encrypt(credentials.AccessKeySecret),
                StringEncryptionService.Encrypt(credentials.SecurityToken));

            Cache.Set(configuration.TemporaryCredentialsCacheKey, temporaryCredentialsCache,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(configuration.DurationSeconds - 10)
                });

            return temporaryCredentialsCache;
        }

    }
}
