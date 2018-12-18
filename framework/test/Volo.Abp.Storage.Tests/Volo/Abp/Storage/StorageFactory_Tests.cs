using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using Volo.Abp.Modularity;
using Volo.Abp.Storage.Azure;
using Volo.Abp.Storage.Configuration;
using Volo.Abp.Storage.FileSystem;
using Volo.Abp.Storage.FileSystem.ExtendedProperties;
using Volo.Abp.Storage.FileSystem.Server;
using Xunit;

namespace Volo.Abp.Storage
{
    public class StorageFactory_Tests : AbpIntegratedTest<StorageFactory_Tests.TestModule>
    {
        private readonly IAbpStorageFactory _storageFactory;

        public StorageFactory_Tests()
        {
            _storageFactory = GetRequiredService<IAbpStorageFactory>();
        }

        [InlineData("FileSystemTestStore1")]
        [InlineData("FileSystemTestStore2")]
        [InlineData("AzureTestStore1")]
        [InlineData("AzureTestStore2")]
        [InlineData("AzureTestStore3")]
        [InlineData("AzureTestStore4")]
        [Theory]
        public void Should_Get_TestStore(string storeName)
        {
            var testStore = _storageFactory.GetStore(storeName);

            testStore.ShouldNotBeNull();
        }

        [InlineData("ScopedFileSystemTestStore")]
        [InlineData("ScopedAzureTestStore")]
        [Theory]
        public void Should_Get_ScopedTestStore(string storeName)
        {
            var args = Guid.NewGuid();
            var testStore = _storageFactory.GetScopedStore(storeName, args);

            testStore.ShouldNotBeNull();
        }

        [DependsOn(
            typeof(AbpAzureStorageModule),
            typeof(AbpFileSystemStorageModule),
            typeof(AbpFileSystemStorageExtendedPropertiesModule),
            typeof(AbpFileSystemStorageServerModule))]
        public class TestModule : AbpModule
        {
            public override void PreConfigureServices(ServiceConfigurationContext context)
            {
                var builder = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.test.json", optional: true, reloadOnChange: true);

                var config = builder.Build();

                context.Services.SetConfiguration(config);
            }

            public override void ConfigureServices(ServiceConfigurationContext context)
            {
                context.Services.Configure<AbpStorageOptions>(context.Services.GetConfiguration().GetSection("AppStorage"));
            }
        }
    }
}
