using System;

namespace Volo.Abp.BlobStoring.Aliyun
{
    /// <summary>
    /// Sub-account access to OSS or STS temporary authorization to access OSS
    /// 子账号/STS临时授权访问OSS
    /// https://help.aliyun.com/document_detail/100624.html
    /// </summary>
    public class AliyunBlobProviderConfiguration
    {
        public string AccessKeyId
        {
            get => _containerConfiguration.GetConfiguration<string>(AliyunBlobProviderConfigurationNames.AccessKeyId);
            set => _containerConfiguration.SetConfiguration(AliyunBlobProviderConfigurationNames.AccessKeyId, Check.NotNullOrWhiteSpace(value, nameof(value)));
        }

        public string AccessKeySecret
        {
            get => _containerConfiguration.GetConfiguration<string>(AliyunBlobProviderConfigurationNames.AccessKeySecret);
            set => _containerConfiguration.SetConfiguration(AliyunBlobProviderConfigurationNames.AccessKeySecret, Check.NotNullOrWhiteSpace(value, nameof(value)));
        }

        /// <summary>
        /// https://help.aliyun.com/document_detail/31837.html
        /// eg: https://oss-cn-beijing.aliyuncs.com
        /// </summary>
        public string Endpoint
        {
            get => _containerConfiguration.GetConfiguration<string>(AliyunBlobProviderConfigurationNames.Endpoint);
            set => _containerConfiguration.SetConfiguration(AliyunBlobProviderConfigurationNames.Endpoint, Check.NotNullOrWhiteSpace(value, nameof(value)));
        }

        /// <summary>
        /// STS https://help.aliyun.com/document_detail/66053.html
        /// eg:cn-beijing
        /// </summary>
        public string RegionId
        {
            get => _containerConfiguration.GetConfiguration<string>(AliyunBlobProviderConfigurationNames.RegionId);
            set => _containerConfiguration.SetConfiguration(AliyunBlobProviderConfigurationNames.RegionId, value);
        }

        /// <summary>
        /// eg:acs:ram::$accountID:role/$roleName
        /// </summary>
        public string RoleArn
        {
            get => _containerConfiguration.GetConfiguration<string>(AliyunBlobProviderConfigurationNames.RoleArn);
            set => _containerConfiguration.SetConfiguration(AliyunBlobProviderConfigurationNames.RoleArn, value);
        }

        /// <summary>
        /// The name used to identify the temporary access credentials, it is recommended to use different application users to distinguish.
        /// 用来标识临时访问凭证的名称，建议使用不同的应用程序用户来区分。
        /// </summary>
        public string RoleSessionName
        {
            get => _containerConfiguration.GetConfiguration<string>(AliyunBlobProviderConfigurationNames.RoleSessionName);
            set => _containerConfiguration.SetConfiguration(AliyunBlobProviderConfigurationNames.RoleSessionName, value);
        }

        /// <summary>
        /// Set the validity period of the temporary access credential, the unit is s, the minimum is 900, and the maximum is 3600.
        /// 设置临时访问凭证的有效期，单位是s，最小为900，最大为3600。
        /// </summary>
        public int DurationSeconds
        {
            get => _containerConfiguration.GetConfigurationOrDefault(AliyunBlobProviderConfigurationNames.DurationSeconds, 0);
            set => _containerConfiguration.SetConfiguration(AliyunBlobProviderConfigurationNames.DurationSeconds, value);
        }

        public string Policy
        {
            get => _containerConfiguration.GetConfiguration<string>(AliyunBlobProviderConfigurationNames.Policy);
            set => _containerConfiguration.SetConfiguration(AliyunBlobProviderConfigurationNames.Policy, value);
        }

        /// <summary>
        /// Default value: false.
        /// </summary>
        public bool CreateContainerIfNotExists
        {
            get => _containerConfiguration.GetConfigurationOrDefault(AliyunBlobProviderConfigurationNames.CreateContainerIfNotExists, false);
            set => _containerConfiguration.SetConfiguration(AliyunBlobProviderConfigurationNames.CreateContainerIfNotExists, value);
        }

        private readonly BlobContainerConfiguration _containerConfiguration;

        public AliyunBlobProviderConfiguration(BlobContainerConfiguration containerConfiguration)
        {
            _containerConfiguration = containerConfiguration;
        }

        public string ToOssKeyString()
        {
            Uri uPoint = new Uri(Endpoint);
            return $"memorycache:aliyun:id:{AccessKeyId},sec:{AccessKeySecret},ept:{uPoint.Host.ToLower()}";
        }

        public string ToOssWithStsKeyString()
        {
            return ToOssKeyString() + $",rid:{RegionId},ra:{RoleArn},rsn:{RoleSessionName},pl:{Policy}";
        }
    }
}
