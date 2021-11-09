namespace Volo.Abp.BlobStoring.Azure;

public class AzureBlobProviderConfiguration
{
    public string ConnectionString
    {
        get => _containerConfiguration.GetConfiguration<string>(AzureBlobProviderConfigurationNames.ConnectionString);
        set => _containerConfiguration.SetConfiguration(AzureBlobProviderConfigurationNames.ConnectionString, Check.NotNullOrWhiteSpace(value, nameof(value)));
    }

    /// <summary>
    /// This name may only contain lowercase letters, numbers, and hyphens, and must begin with a letter or a number.
    /// Each hyphen must be preceded and followed by a non-hyphen character.
    /// The name must also be between 3 and 63 characters long.
    /// If this parameter is not specified, the ContainerName of the <see cref="BlobProviderArgs"/> will be used.
    /// </summary>
    public string ContainerName
    {
        get => _containerConfiguration.GetConfigurationOrDefault<string>(AzureBlobProviderConfigurationNames.ContainerName);
        set => _containerConfiguration.SetConfiguration(AzureBlobProviderConfigurationNames.ContainerName, value);
    }

    /// <summary>
    /// Default value: false.
    /// </summary>
    public bool CreateContainerIfNotExists
    {
        get => _containerConfiguration.GetConfigurationOrDefault(AzureBlobProviderConfigurationNames.CreateContainerIfNotExists, false);
        set => _containerConfiguration.SetConfiguration(AzureBlobProviderConfigurationNames.CreateContainerIfNotExists, value);
    }

    private readonly BlobContainerConfiguration _containerConfiguration;

    public AzureBlobProviderConfiguration(BlobContainerConfiguration containerConfiguration)
    {
        _containerConfiguration = containerConfiguration;
    }
}
