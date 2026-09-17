namespace Volo.Abp.BlobStoring.Bunny;

public class BunnyBlobProviderConfiguration
{
    public string? Region {
        get => _containerConfiguration.GetConfigurationOrDefault(BunnyBlobProviderConfigurationNames.Region, "de");
        set => _containerConfiguration.SetConfiguration(BunnyBlobProviderConfigurationNames.Region, value);
    }

    /// <summary>
    /// This name may only contain lowercase letters, numbers, and hyphens. (no spaces)
    /// The name must also be between 4 and 64 characters long.
    /// The name must be globaly unique 
    /// If this parameter is not specified, the ContainerName of the <see cref="BlobProviderArgs"/> will be used.
    /// </summary>
    public string? ContainerName {
        get => _containerConfiguration.GetConfigurationOrDefault<string>(BunnyBlobProviderConfigurationNames.ContainerName);
        set => _containerConfiguration.SetConfiguration(BunnyBlobProviderConfigurationNames.ContainerName, value);
    }

    /// <summary>
    /// Default value: false.
    /// </summary>
    public bool CreateContainerIfNotExists {
        get => _containerConfiguration.GetConfigurationOrDefault(BunnyBlobProviderConfigurationNames.CreateContainerIfNotExists, false);
        set => _containerConfiguration.SetConfiguration(BunnyBlobProviderConfigurationNames.CreateContainerIfNotExists, value);
    }

    public string AccessKey {
        get => _containerConfiguration.GetConfiguration<string>(BunnyBlobProviderConfigurationNames.AccessKey);
        set => _containerConfiguration.SetConfiguration(BunnyBlobProviderConfigurationNames.AccessKey, value);
    }

    private readonly BlobContainerConfiguration _containerConfiguration;

    public BunnyBlobProviderConfiguration(BlobContainerConfiguration containerConfiguration)
    {
        _containerConfiguration = containerConfiguration;
    }
}