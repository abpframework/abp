using System;

namespace Volo.Abp.Storage.Amazon
{
    public class AmazonProviderOptions
    {
        public string PublicKey { get; set; }

        public string SecretKey { get; set; }

        public string Bucket { get; set; }

        public string ServiceUrl { get; set; }

        public string ServerSideEncryptionMethod { get; set; }

        public string ProfileName { get; set; }

        public TimeSpan? Timeout { get; set; }
    }
}