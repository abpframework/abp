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
    /// STS:https://help.aliyun.com/document_detail/28756.html
    /// STS or sub account number
    /// </summary>
    public class DefaultOssClientFactory : IOssClientFactory, ITransientDependency
    {
        private readonly IBlobContainerConfigurationProvider _configurationProvider;
        private readonly IMemoryCache _cache;
        public DefaultOssClientFactory(
            IBlobContainerConfigurationProvider configurationProvider,
            IMemoryCache cache)
        {
            _configurationProvider = configurationProvider;
            _cache = cache;
        }

        public virtual IOss Create(AliyunBlobProviderConfiguration aliyunConfig)
        {
            //使用账号 sub account number
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
                //使用STS
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
                //构建AssumeRole请求
                AssumeRoleRequest request = new AssumeRoleRequest
                {
                    AcceptFormat = FormatType.JSON,
                    //指定角色ARN
                    RoleArn = aliyunConfig.RoleArn,
                    RoleSessionName = aliyunConfig.RoleSessionName,
                    //设置Token有效期，可选参数，默认3600秒
                    DurationSeconds = aliyunConfig.DurationSeconds,
                    //设置Token的附加权限策略；在获取Token时，通过额外设置一个权限策略进一步减小Token的权限
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
