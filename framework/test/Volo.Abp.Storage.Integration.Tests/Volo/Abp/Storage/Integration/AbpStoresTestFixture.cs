using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Volo.Abp.Storage.Azure.Configuration;
using Volo.Abp.Storage.Configuration;
using Volo.Abp.Storage.FileSystem.Configuration;
using Volo.Abp.Storage.FileSystem.ExtendedProperties;

namespace Volo.Abp.Storage.Integration
{
    public class AbpStoresTestFixture : IDisposable
    {
        public IConfigurationRoot Configuration { get; }
        public IServiceProvider Services { get; }
        public string BasePath { get; }
        public string FileSystemRootPath => Path.Combine(BasePath, "FileVault");
        public string FileSystemSecondaryRootPath => Path.Combine(BasePath, "FileVault2");
        public AbpStorageOptions StorageOptions { get; }
        public AbpAzureParsedOptions AzureParsedOptions { get; }
        public AbpFileSystemParsedOptions FileSystemParsedOptions { get; }
        public AbpFileSystemStoreOptions TestStoreOptions { get; }

        public AbpStoresTestFixture()
        {
            BasePath = PlatformServices.Default.Application.ApplicationBasePath;

            var containerId = Guid.NewGuid().ToString("N").ToLower();

            var builder = new ConfigurationBuilder()
                .SetBasePath(BasePath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.development.json", optional: true)
                .AddInMemoryCollection(new KeyValuePair<string, string>[]
                {
                    new KeyValuePair<string, string>("Storage:Stores:Store3:FolderName", $"Store3-{containerId}"),
                    new KeyValuePair<string, string>("Storage:Stores:Store4:FolderName", $"Store4-{containerId}"),
                    new KeyValuePair<string, string>("Storage:Stores:Store5:FolderName", $"Store5-{containerId}"),
                    new KeyValuePair<string, string>("Storage:Stores:Store6:FolderName", $"Store6-{containerId}"),
                });

            Configuration = builder.Build();

            var services = new ServiceCollection();

            services.AddOptions();

            services.AddAbpStorage(Configuration)
                .AddAzureStorage()
                .AddFileSystemStorage(FileSystemRootPath)
                .AddFileSystemExtendedProperties();

            Services = services.BuildServiceProvider();

            StorageOptions = Services.GetService<IOptions<AbpStorageOptions>>().Value;

            AzureParsedOptions = Services.GetService<IOptions<AbpAzureParsedOptions>>().Value;

            FileSystemParsedOptions = Services.GetService<IOptions<AbpFileSystemParsedOptions>>().Value;

            TestStoreOptions = Services.GetService<IOptions<TestStore>>().Value
                .ParseStoreOptions<AbpFileSystemParsedOptions, AbpFileSystemProviderInstanceOptions,
                    AbpFileSystemStoreOptions, AbpFileSystemScopedStoreOptions>(FileSystemParsedOptions);

            ResetStores();
        }

        public void Dispose()
        {
            DeleteRootResources();
        }

        private void DeleteRootResources()
        {
            foreach (var parsedStoreKvp in AzureParsedOptions.ParsedStores)
            {
                var cloudStorageAccount = CloudStorageAccount.Parse(parsedStoreKvp.Value.ConnectionString);
                var client = cloudStorageAccount.CreateCloudBlobClient();
                var container = client.GetContainerReference(parsedStoreKvp.Value.ContainerName);

                container.DeleteIfExistsAsync().Wait();
            }

            if (Directory.Exists(FileSystemRootPath))
            {
                Directory.Delete(FileSystemRootPath, true);
            }

            if (Directory.Exists(FileSystemSecondaryRootPath))
            {
                Directory.Delete(FileSystemSecondaryRootPath, true);
            }
        }

        private void ResetStores()
        {
            DeleteRootResources();
            ResetAzureStores();
            ResetFileSystemStores();
        }

        private void ResetFileSystemStores()
        {
            if (!Directory.Exists(FileSystemRootPath))
            {
                Directory.CreateDirectory(FileSystemRootPath);
            }

            foreach (var parsedStoreKvp in FileSystemParsedOptions.ParsedStores)
            {
                ResetFileSystemStore(parsedStoreKvp.Key, parsedStoreKvp.Value.AbsolutePath);
            }

            ResetFileSystemStore(TestStoreOptions.Name, TestStoreOptions.AbsolutePath);
        }

        private void ResetFileSystemStore(string storeName, string absolutePath)
        {
            var process = Process.Start(new ProcessStartInfo("robocopy.exe")
            {
                Arguments = $"\"{Path.Combine(BasePath, "SampleDirectory")}\" \"{absolutePath}\" /MIR"
            });

            if (process.WaitForExit(30000)) return;
            process.Kill();
            throw new TimeoutException($"FileSystem Store '{storeName}' was not reset properly.");
        }

        private void ResetAzureStores()
        {
            var azCopy = Path.Combine(
                Environment.ExpandEnvironmentVariables("%ProgramFiles(x86)%\\Microsoft SDKs\\Azure\\AzCopy"),
                "AzCopy.exe");

            foreach (var parsedStoreKvp in AzureParsedOptions.ParsedStores)
            {
                var cloudStorageAccount = CloudStorageAccount.Parse(parsedStoreKvp.Value.ConnectionString);
                var cloudStoragekey = cloudStorageAccount.Credentials.ExportBase64EncodedKey();
                var containerName = parsedStoreKvp.Value.ContainerName;

                var dest = cloudStorageAccount.BlobStorageUri.PrimaryUri.ToString() + containerName;

                var client = cloudStorageAccount.CreateCloudBlobClient();

                var container = client.GetContainerReference(containerName);
                container.CreateIfNotExistsAsync().Wait();

                var arguments =
                    $"/Source:\"{Path.Combine(BasePath, "SampleDirectory")}\" /Dest:\"{dest}\" /DestKey:{cloudStoragekey} /S /y";
                var process = Process.Start(new ProcessStartInfo(azCopy)
                {
                    Arguments = arguments
                });

                if (process.WaitForExit(30000)) continue;
                process.Kill();
                throw new TimeoutException($"Azure Store '{parsedStoreKvp.Key}' was not reset properly.");
            }
        }
    }
}