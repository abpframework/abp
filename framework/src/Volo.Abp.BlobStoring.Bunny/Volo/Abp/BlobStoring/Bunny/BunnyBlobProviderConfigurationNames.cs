namespace Volo.Abp.BlobStoring.Bunny;

public static class BunnyBlobProviderConfigurationNames
{
    // The primary region for the storage zone (e.g., DE, NY, etc.)
    public const string Region = "Bunny.Region";

    // The name of the storage zone
    public const string ContainerName = "Bunny.ContainerName";

    // The API access key for the bunny.net account
    public const string AccessKey = "Bunny.AccessKey";

    public const string CreateContainerIfNotExists = "Bunny.CreateContainerIfNotExists";
}
