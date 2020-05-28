using System;
using System.IO;
using Shouldly;
using Xunit;

namespace Volo.Abp.BlobStoring.FileSystem
{
    public class BlogFilePathCalculator_Tests : AbpBlobStoringFileSystemTestBase
    {
        private readonly IBlogFilePathCalculator _calculator;

        public BlogFilePathCalculator_Tests()
        {
            _calculator = GetRequiredService<IBlogFilePathCalculator>();
        }

        [Fact]
        public void Default_Settings()
        {
            var separator = Path.DirectorySeparatorChar;

            _calculator.Calculate(
                GetArgs($"C:{separator}my-files", "my-container", "my-blob")
            ).ShouldBe($"C:{separator}my-files{separator}host{separator}my-container{separator}my-blob");
        }

        [Fact]
        public void Default_Settings_With_TenantId()
        {
            var separator = Path.DirectorySeparatorChar;
            var tenantId = Guid.NewGuid();

            _calculator.Calculate(
                GetArgs($"C:{separator}my-files", "my-container", "my-blob", tenantId: tenantId)
            ).ShouldBe($"C:{separator}my-files{separator}tenants{separator}{tenantId:D}{separator}my-container{separator}my-blob");
        }

        [Fact]
        public void AppendContainerNameToBasePath_Set_To_False()
        {
            var separator = Path.DirectorySeparatorChar;

            _calculator.Calculate(
                GetArgs($"C:{separator}my-files", "my-container", "my-blob", appendContainerNameToBasePath: false)
            ).ShouldBe($"C:{separator}my-files{separator}host{separator}my-blob");
        }

        private static BlobProviderArgs GetArgs(
            string basePath,
            string containerName,
            string blobName,
            Guid? tenantId = null,
            bool? appendContainerNameToBasePath = null)
        {
            return new BlobProviderGetArgs(
                containerName,
                new BlobContainerConfiguration()
                    .UseFileSystem(fs =>
                    {
                        fs.BasePath = basePath;
                        if (appendContainerNameToBasePath.HasValue)
                        {
                            fs.AppendContainerNameToBasePath = appendContainerNameToBasePath.Value;
                        }
                    }),
                blobName,
                tenantId
            );
        }
    }
}