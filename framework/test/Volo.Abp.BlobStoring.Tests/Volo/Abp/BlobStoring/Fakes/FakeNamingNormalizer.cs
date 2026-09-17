namespace Volo.Abp.BlobStoring.Fakes;

public class FakeNamingNormalizer : IBlobNamingNormalizer
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
