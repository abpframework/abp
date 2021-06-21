namespace Volo.Abp.BlobStoring.Minio
{
    public static class MinioBlobProviderConfigurationNames
    {
        public const string BucketName = "Minio.BucketName";
        public const string EndPoint = "Minio.EndPoint";
        public const string AccessKey = "Minio.AccessKey";
        public const string SecretKey = "Minio.SecretKey";
        public const string WithSSL = "Minio.WithSSL";
        public const string CreateBucketIfNotExists = "Minio.CreateBucketIfNotExists";
    }
}
