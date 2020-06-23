namespace Volo.Abp.BlobStoring
{
    public interface IBlobNamingNormalizerProvider
    {
        string NormalizeContainerName(string containerName);

        string NormalizeBlobName(string blobName);
    }
}
