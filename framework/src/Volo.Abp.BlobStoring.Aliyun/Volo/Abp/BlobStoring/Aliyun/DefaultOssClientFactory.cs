using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Auth.Sts;
using Aliyun.Acs.Core.Http;
using Aliyun.Acs.Core.Profile;
using Aliyun.OSS;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlobStoring.Aliyun
{
    /// <summary>
    /// Sub-account access to OSS or STS temporary authorization to access OSS
    /// 子账号/STS临时授权访问OSS
    /// STS:https://help.aliyun.com/document_detail/28756.html
    /// </summary>
    public class DefaultOssClientFactory : IOssClientFactory, ITransientDependency
    {
        private readonly IMemoryCache _cache;
        public DefaultOssClientFactory(
            IMemoryCache cache)
        {
            _cache = cache;
        }

        public virtual IOss Create(AliyunBlobProviderConfiguration aliyunConfig)
        {
            //Sub-account
            if (aliyunConfig.DurationSeconds <= 0)
            {
                var key = aliyunConfig.ToOssKeyString();
                var iOssClient = _cache.Get<IOss>(key);
                if (iOssClient != null)
                {
                    return iOssClient;
                }
                iOssClient = new OssClient(aliyunConfig.Endpoint, aliyunConfig.AccessKeyId, aliyunConfig.AccessKeySecret);
                _cache.Set(key, iOssClient);
                return iOssClient;
            }
            else
            {
                //STS temporary authorization to access OSS
                var key = aliyunConfig.ToOssWithStsKeyString();
                var iOssClient = _cache.Get<IOss>(key);
                if (iOssClient != null)
                {
                    return iOssClient;
                }
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
                iOssClient = new OssClient(aliyunConfig.Endpoint, response.Credentials.AccessKeyId, response.Credentials.AccessKeySecret, response.Credentials.SecurityToken);
                _cache.Set(key, iOssClient, TimeSpan.FromSeconds(aliyunConfig.DurationSeconds));
                return iOssClient;
            }
        }
    }
}
