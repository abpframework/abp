using System;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.BlobStoring.Google;


/// <summary>
/// This module will not try to connect to Google Cloud Storage.
/// </summary>
[DependsOn(
    typeof(AbpBlobStoringGoogleModule),
    typeof(AbpBlobStoringTestModule)
)]
public class AbpBlobStoringGoogleTestCommonModule : AbpModule
{

}

[DependsOn(
    typeof(AbpBlobStoringGoogleTestCommonModule)
)]
public class AbpBlobStoringGoogleTestModule : AbpModule
{
    private const string UserSecretsId = "9f0d2c00-80c1-435b-bfab-2c39c8249091";

    private string _clientEmail;
    private string _projectId;
    private string _privateKey;

    private readonly string _randomContainerName = "abp-test-container-" + Guid.NewGuid().ToString("N");

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.ReplaceConfiguration(ConfigurationHelper.BuildConfiguration(builderAction: builder =>
        {
            builder.AddUserSecrets(UserSecretsId);
        }));

        var configuration = context.Services.GetConfiguration();
        _clientEmail = configuration["Google:ClientEmail"];
        _projectId = configuration["Google:ProjectId"];
        _privateKey = configuration["Google:PrivateKey"];
        
        Configure<AbpBlobStoringOptions>(options =>
        {
            options.Containers.ConfigureAll((containerName, containerConfiguration) =>
            {
                containerConfiguration.UseGoogle(google =>
                {
                    google.ClientEmail = _clientEmail;
                    google.ProjectId = _projectId;
                    google.PrivateKey = _privateKey;
                    google.ContainerName = _randomContainerName;
                    google.CreateContainerIfNotExists = true;
                });
            });
        });
    }

    public override void OnApplicationShutdown(ApplicationShutdownContext context)
    {
        var googleCredential = GoogleCredential.FromServiceAccountCredential(
            new ServiceAccountCredential(
                new ServiceAccountCredential.Initializer(_clientEmail)
                    {
                        ProjectId = _projectId
                    }
                    .FromPrivateKey(_privateKey)
            ));

        var client =  StorageClient.Create(googleCredential);

        try
        {
            client.DeleteBucket(_randomContainerName, new DeleteBucketOptions
            {
                DeleteObjects = true
            });
        }
        catch (Exception e)
        {
            // ignored
        }
    }
}