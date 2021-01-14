namespace Volo.Abp.BlobStoring
{
    public interface IBlobNormalizeNamingFactory
    {
        (string containerName, string blobName) NormalizeNaming(BlobContainerConfiguration configuration, string containerName, string blobName);

        string NormalizeContainerName(BlobContainerConfiguration configuration, string containerName);

        string NormalizeBlobName(BlobContainerConfiguration configuration, string blobName);
    }
}
