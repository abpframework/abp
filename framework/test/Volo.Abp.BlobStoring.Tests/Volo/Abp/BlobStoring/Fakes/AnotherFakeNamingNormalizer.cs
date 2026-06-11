namespace Volo.Abp.BlobStoring.Fakes;

public class AnotherFakeNamingNormalizer : IBlobNamingNormalizer
{
    public string NormalizeContainerName(string containerName)
    {
        return containerName;
    }

    public string NormalizeBlobName(string blobName)
    {
        return blobName;
    }
}
