using System;

namespace Volo.Abp.BlobStoring.Aws;

public class AwsBlobProviderConfiguration
{
    public string? AccessKeyId {
        get => _containerConfiguration.GetConfigurationOrDefault<string>(AwsBlobProviderConfigurationNames.AccessKeyId);
        set => _containerConfiguration.SetConfiguration(AwsBlobProviderConfigurationNames.AccessKeyId, value);
    }

    public string? SecretAccessKey {
        get => _containerConfiguration.GetConfigurationOrDefault<string>(AwsBlobProviderConfigurationNames.SecretAccessKey);
        set => _containerConfiguration.SetConfiguration(AwsBlobProviderConfigurationNames.SecretAccessKey, value);
    }

    public bool UseCredentials {
        get => _containerConfiguration.GetConfigurationOrDefault(AwsBlobProviderConfigurationNames.UseCredentials, false);
        set => _containerConfiguration.SetConfiguration(AwsBlobProviderConfigurationNames.UseCredentials, value);
    }

    public bool UseTemporaryCredentials {
        get => _containerConfiguration.GetConfigurationOrDefault(AwsBlobProviderConfigurationNames.UseTemporaryCredentials, false);
        set => _containerConfiguration.SetConfiguration(AwsBlobProviderConfigurationNames.UseTemporaryCredentials, value);
    }

    public bool UseTemporaryFederatedCredentials {
        get => _containerConfiguration.GetConfigurationOrDefault(AwsBlobProviderConfigurationNames.UseTemporaryFederatedCredentials, false);
        set => _containerConfiguration.SetConfiguration(AwsBlobProviderConfigurationNames.UseTemporaryFederatedCredentials, value);
    }

    public string? ProfileName {
        get => _containerConfiguration.GetConfigurationOrDefault<string>(AwsBlobProviderConfigurationNames.ProfileName);
        set => _containerConfiguration.SetConfiguration(AwsBlobProviderConfigurationNames.ProfileName, value);
    }

    public string? ProfilesLocation {
        get => _containerConfiguration.GetConfigurationOrDefault<string>(AwsBlobProviderConfigurationNames.ProfilesLocation);
        set => _containerConfiguration.SetConfiguration(AwsBlobProviderConfigurationNames.ProfilesLocation, value);
    }

    /// <summary>
    /// Set the validity period of the temporary access credential, the unit is s, the minimum is 900, and the maximum is 129600.
    /// </summary>
    public int DurationSeconds {
        get => _containerConfiguration.GetConfigurationOrDefault(AwsBlobProviderConfigurationNames.DurationSeconds, 0);
        set => _containerConfiguration.SetConfiguration(AwsBlobProviderConfigurationNames.DurationSeconds, value);
    }

    public string Name {
        get => _containerConfiguration.GetConfiguration<string>(AwsBlobProviderConfigurationNames.Name);
        set => _containerConfiguration.SetConfiguration(AwsBlobProviderConfigurationNames.Name, value);
    }

    public string? Policy {
        get => _containerConfiguration.GetConfigurationOrDefault<string>(AwsBlobProviderConfigurationNames.Policy);
        set => _containerConfiguration.SetConfiguration(AwsBlobProviderConfigurationNames.Policy, value);
    }

    /// <summary>
    /// The system name of the AWS region (e.g., <c>us-east-1</c>). Required for real AWS S3.
    /// Optional when <see cref="ServiceURL"/> is configured for an S3-compatible service;
    /// some services accept any value (or <c>auto</c> for Cloudflare R2).
    /// </summary>
    public string? Region {
        get => _containerConfiguration.GetConfigurationOrDefault<string>(AwsBlobProviderConfigurationNames.Region);
        set
        {
            if (value == null)
            {
                _containerConfiguration.ClearConfiguration(AwsBlobProviderConfigurationNames.Region);
                return;
            }
            _containerConfiguration.SetConfiguration(AwsBlobProviderConfigurationNames.Region, value);
        }
    }

    /// <summary>
    /// Custom service URL for S3-compatible APIs (e.g., MinIO, DigitalOcean Spaces, Cloudflare R2).
    /// When omitted, <see cref="Region"/> becomes required and the default AWS S3 endpoint for that
    /// region is used. The AWS SDK automatically appends a trailing slash to the configured value.
    /// </summary>
    public string? ServiceURL {
        get => _containerConfiguration.GetConfigurationOrDefault<string>(AwsBlobProviderConfigurationNames.ServiceURL);
        set
        {
            if (value == null)
            {
                _containerConfiguration.ClearConfiguration(AwsBlobProviderConfigurationNames.ServiceURL);
                return;
            }
            _containerConfiguration.SetConfiguration(AwsBlobProviderConfigurationNames.ServiceURL, value);
        }
    }

    /// <summary>
    /// When true, payload signing is disabled on PutObject upload requests so the SDK sends
    /// <c>x-amz-content-sha256: UNSIGNED-PAYLOAD</c> instead of the streaming chunked signature
    /// (<c>STREAMING-AWS4-HMAC-SHA256-PAYLOAD</c>) that AWS SDK v4 uses by default. Required for
    /// Cloudflare R2 and other S3-compatible services that do not implement streaming signing.
    /// The endpoint must be HTTPS when this option is enabled (AWS SDK rejects unsigned payloads
    /// over HTTP). Default: false (keep AWS SDK defaults for real AWS S3 and MinIO).
    /// </summary>
    public bool DisablePayloadSigning {
        get => _containerConfiguration.GetConfigurationOrDefault(AwsBlobProviderConfigurationNames.DisablePayloadSigning, false);
        set => _containerConfiguration.SetConfiguration(AwsBlobProviderConfigurationNames.DisablePayloadSigning, value);
    }

    /// <summary>
    /// This name may only contain lowercase letters, numbers, and hyphens, and must begin with a letter or a number.
    /// Each hyphen must be preceded and followed by a non-hyphen character.
    /// The name must also be between 3 and 63 characters long.
    /// If this parameter is not specified, the ContainerName of the <see cref="BlobProviderArgs"/> will be used.
    /// </summary>
    public string? ContainerName {
        get => _containerConfiguration.GetConfigurationOrDefault<string>(AwsBlobProviderConfigurationNames.ContainerName);
        set => _containerConfiguration.SetConfiguration(AwsBlobProviderConfigurationNames.ContainerName, value);
    }

    /// <summary>
    /// Default value: false.
    /// </summary>
    public bool CreateContainerIfNotExists {
        get => _containerConfiguration.GetConfigurationOrDefault(AwsBlobProviderConfigurationNames.CreateContainerIfNotExists, false);
        set => _containerConfiguration.SetConfiguration(AwsBlobProviderConfigurationNames.CreateContainerIfNotExists, value);
    }

    private readonly string _temporaryCredentialsCacheKey;
    public string? TemporaryCredentialsCacheKey {
        get => _containerConfiguration.GetConfigurationOrDefault(AwsBlobProviderConfigurationNames.TemporaryCredentialsCacheKey, _temporaryCredentialsCacheKey);
        set => _containerConfiguration.SetConfiguration(AwsBlobProviderConfigurationNames.TemporaryCredentialsCacheKey, value);
    }

    private readonly BlobContainerConfiguration _containerConfiguration;

    public AwsBlobProviderConfiguration(BlobContainerConfiguration containerConfiguration)
    {
        _containerConfiguration = containerConfiguration;
        _temporaryCredentialsCacheKey = Guid.NewGuid().ToString("N");
    }
}
