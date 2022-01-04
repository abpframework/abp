using System;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Encryption;

namespace Volo.Abp.BlobStoring.Aws;

public class DefaultAmazonS3ClientFactory : IAmazonS3ClientFactory, ITransientDependency
{
    protected IDistributedCache<AwsTemporaryCredentialsCacheItem> Cache { get; }

    protected IStringEncryptionService StringEncryptionService { get; }

    public DefaultAmazonS3ClientFactory(
        IDistributedCache<AwsTemporaryCredentialsCacheItem> cache,
        IStringEncryptionService stringEncryptionService)
    {
        Cache = cache;
        StringEncryptionService = stringEncryptionService;
    }

    public virtual async Task<AmazonS3Client> GetAmazonS3Client(
        AwsBlobProviderConfiguration configuration)
    {
        var region = RegionEndpoint.GetBySystemName(configuration.Region);

        if (configuration.UseCredentials)
        {
            var awsCredentials = GetAwsCredentials(configuration);
            return awsCredentials == null
                ? new AmazonS3Client(region)
                : new AmazonS3Client(awsCredentials, region);
        }

        if (configuration.UseTemporaryCredentials)
        {
            return new AmazonS3Client(await GetTemporaryCredentialsAsync(configuration), region);
        }

        if (configuration.UseTemporaryFederatedCredentials)
        {
            return new AmazonS3Client(await GetTemporaryFederatedCredentialsAsync(configuration),
                region);
        }

        Check.NotNullOrWhiteSpace(configuration.AccessKeyId, nameof(configuration.AccessKeyId));
        Check.NotNullOrWhiteSpace(configuration.SecretAccessKey, nameof(configuration.SecretAccessKey));

        return new AmazonS3Client(configuration.AccessKeyId, configuration.SecretAccessKey, region);
    }

    protected virtual AWSCredentials GetAwsCredentials(
        AwsBlobProviderConfiguration configuration)
    {
        if (configuration.ProfileName.IsNullOrWhiteSpace())
        {
            return null;
        }

        var chain = new CredentialProfileStoreChain(configuration.ProfilesLocation);

        if (chain.TryGetAWSCredentials(configuration.ProfileName, out var awsCredentials))
        {
            return awsCredentials;
        }

        throw new AmazonS3Exception("Not found aws credentials");
    }

    protected virtual async Task<SessionAWSCredentials> GetTemporaryCredentialsAsync(
        AwsBlobProviderConfiguration configuration)
    {
        var temporaryCredentialsCache = await Cache.GetAsync(configuration.TemporaryCredentialsCacheKey);

        if (temporaryCredentialsCache == null)
        {
            AmazonSecurityTokenServiceClient stsClient;

            if (!configuration.AccessKeyId.IsNullOrEmpty() && !configuration.SecretAccessKey.IsNullOrEmpty())
            {
                stsClient = new AmazonSecurityTokenServiceClient(configuration.AccessKeyId,
                    configuration.SecretAccessKey);
            }
            else
            {
                var awsCredentials = GetAwsCredentials(configuration);
                stsClient = awsCredentials == null
                    ? new AmazonSecurityTokenServiceClient()
                    : new AmazonSecurityTokenServiceClient(awsCredentials);
            }

            using (stsClient)
            {
                var getSessionTokenRequest = new GetSessionTokenRequest
                {
                    DurationSeconds = configuration.DurationSeconds
                };

                var sessionTokenResponse =
                    await stsClient.GetSessionTokenAsync(getSessionTokenRequest);

                var credentials = sessionTokenResponse.Credentials;

                temporaryCredentialsCache =
                    await SetTemporaryCredentialsCache(configuration, credentials);
            }
        }

        var sessionCredentials = new SessionAWSCredentials(
            StringEncryptionService.Decrypt(temporaryCredentialsCache.AccessKeyId),
            StringEncryptionService.Decrypt(temporaryCredentialsCache.SecretAccessKey),
            StringEncryptionService.Decrypt(temporaryCredentialsCache.SessionToken));
        return sessionCredentials;
    }

    protected virtual async Task<SessionAWSCredentials> GetTemporaryFederatedCredentialsAsync(
        AwsBlobProviderConfiguration configuration)
    {
        Check.NotNullOrWhiteSpace(configuration.Name, nameof(configuration.Name));
        Check.NotNullOrWhiteSpace(configuration.Policy, nameof(configuration.Policy));

        var temporaryCredentialsCache = await Cache.GetAsync(configuration.TemporaryCredentialsCacheKey);

        if (temporaryCredentialsCache == null)
        {
            AmazonSecurityTokenServiceClient stsClient;

            if (!configuration.AccessKeyId.IsNullOrEmpty() && !configuration.SecretAccessKey.IsNullOrEmpty())
            {
                stsClient = new AmazonSecurityTokenServiceClient(configuration.AccessKeyId,
                    configuration.SecretAccessKey);
            }
            else
            {
                var awsCredentials = GetAwsCredentials(configuration);
                stsClient = awsCredentials == null
                    ? new AmazonSecurityTokenServiceClient()
                    : new AmazonSecurityTokenServiceClient(awsCredentials);
            }

            using (stsClient)
            {
                var federationTokenRequest =
                    new GetFederationTokenRequest
                    {
                        DurationSeconds = configuration.DurationSeconds,
                        Name = configuration.Name,
                        Policy = configuration.Policy
                    };

                var federationTokenResponse =
                    await stsClient.GetFederationTokenAsync(federationTokenRequest);
                var credentials = federationTokenResponse.Credentials;

                temporaryCredentialsCache =
                    await SetTemporaryCredentialsCache(configuration, credentials);
            }
        }

        var sessionCredentials = new SessionAWSCredentials(
            StringEncryptionService.Decrypt(temporaryCredentialsCache.AccessKeyId),
            StringEncryptionService.Decrypt(temporaryCredentialsCache.SecretAccessKey),
            StringEncryptionService.Decrypt(temporaryCredentialsCache.SessionToken));
        return sessionCredentials;
    }

    private async Task<AwsTemporaryCredentialsCacheItem> SetTemporaryCredentialsCache(
        AwsBlobProviderConfiguration configuration,
        Credentials credentials)
    {
        var temporaryCredentialsCache = new AwsTemporaryCredentialsCacheItem(
            StringEncryptionService.Encrypt(credentials.AccessKeyId),
            StringEncryptionService.Encrypt(credentials.SecretAccessKey),
            StringEncryptionService.Encrypt(credentials.SessionToken));

        await Cache.SetAsync(configuration.TemporaryCredentialsCacheKey, temporaryCredentialsCache,
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(configuration.DurationSeconds - 10)
            });

        return temporaryCredentialsCache;
    }
}
