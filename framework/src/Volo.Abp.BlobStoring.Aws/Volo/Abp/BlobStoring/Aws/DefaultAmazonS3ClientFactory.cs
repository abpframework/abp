using System;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlobStoring.Aws
{
    public class DefaultAmazonS3ClientFactory : IAmazonS3ClientFactory, ITransientDependency
    {
        protected IDistributedCache<TemporaryCredentialsCacheItem> Cache { get; }

        public DefaultAmazonS3ClientFactory(IDistributedCache<TemporaryCredentialsCacheItem> cache)
        {
            Cache = cache;
        }

        public virtual async Task<AmazonS3Client> GetAmazonS3Client(
            AwsBlobProviderConfiguration configuration)
        {
            if (configuration.UseAwsCredentials)
            {
                return new AmazonS3Client(GetAwsCredentials(configuration), configuration.Region);
            }

            if (configuration.UseTemporaryCredentials)
            {
                return new AmazonS3Client(await GetTemporaryCredentialsAsync(configuration), configuration.Region);
            }

            if (configuration.UseTemporaryFederatedCredentials)
            {
                return new AmazonS3Client(await GetTemporaryFederatedCredentialsAsync(configuration),
                    configuration.Region);
            }

            Check.NotNullOrWhiteSpace(configuration.AccessKeyId, nameof(configuration.AccessKeyId));
            Check.NotNullOrWhiteSpace(configuration.SecretAccessKey, nameof(configuration.SecretAccessKey));

            return new AmazonS3Client(configuration.AccessKeyId, configuration.SecretAccessKey);
        }

        protected virtual AWSCredentials GetAwsCredentials(
            AwsBlobProviderConfiguration configuration)
        {
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
            var temporaryCredentialsCache = await Cache.GetAsync(TemporaryCredentialsCacheItem.Key);

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
                    stsClient = new AmazonSecurityTokenServiceClient(GetAwsCredentials(configuration));
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
                        await SetTemporaryCredentialsCache(credentials, configuration.DurationSeconds);
                }
            }

            var sessionCredentials = new SessionAWSCredentials(
                temporaryCredentialsCache.AccessKeyId,
                temporaryCredentialsCache.SecretAccessKey,
                temporaryCredentialsCache.SessionToken);
            return sessionCredentials;
        }

        protected virtual async Task<SessionAWSCredentials> GetTemporaryFederatedCredentialsAsync(
            AwsBlobProviderConfiguration configuration)
        {
            Check.NotNullOrWhiteSpace(configuration.Name, nameof(configuration.Name));

            var temporaryCredentialsCache = await Cache.GetAsync(TemporaryCredentialsCacheItem.Key);

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
                    stsClient = new AmazonSecurityTokenServiceClient(GetAwsCredentials(configuration));
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
                        await SetTemporaryCredentialsCache(credentials, configuration.DurationSeconds);
                }
            }

            var sessionCredentials = new SessionAWSCredentials(
                temporaryCredentialsCache.AccessKeyId,
                temporaryCredentialsCache.SecretAccessKey,
                temporaryCredentialsCache.SessionToken);
            return sessionCredentials;
        }

        private async Task<TemporaryCredentialsCacheItem> SetTemporaryCredentialsCache(
            Credentials credentials,
            int durationSeconds)
        {
            var temporaryCredentialsCache = new TemporaryCredentialsCacheItem(credentials.AccessKeyId,
                credentials.SecretAccessKey,
                credentials.SessionToken);

            await Cache.SetAsync(TemporaryCredentialsCacheItem.Key, temporaryCredentialsCache,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(durationSeconds - 10)
                });

            return temporaryCredentialsCache;
        }
    }
}