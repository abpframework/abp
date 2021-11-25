using System;
using System.Collections.Generic;
using System.Text;

namespace Volo.Abp.BlobStoring.HuaweiCloud
{
    public class HuaweiCloudBlobProviderConfigurationNames
    {
        public const string BucketName = "HuaweiCloud.BucketName";
        public const string EndPoint = "HuaweiCloud.EndPoint";
        public const string AccessKey = "HuaweiCloud.AccessKey";
        public const string SecretKey = "HuaweiCloud.SecretKey";
        public const string WithSSL = "HuaweiCloud.WithSSL";
        public const string CreateBucketIfNotExists = "HuaweiCloud.CreateBucketIfNotExists";
    }
}
