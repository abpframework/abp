using System.Runtime;
using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Volo.Abp.BlobStoring.FileSystem
{
    public class DefaultFileSystemBlobNamingNormalizerProvider_Tests : AbpBlobStoringFileSystemTestBase
    {
        private readonly IBlobNamingNormalizer _blobNamingNormalizer;

        public DefaultFileSystemBlobNamingNormalizerProvider_Tests()
        {
            _blobNamingNormalizer = GetRequiredService<IBlobNamingNormalizer>();
        }

        protected override void AfterAddApplication(IServiceCollection services)
        {
            var _iosPlatformProvider = Substitute.For<IOSPlatformProvider>();
            _iosPlatformProvider.GetCurrentOSPlatform().Returns(OSPlatform.Windows);
            services.AddSingleton(_iosPlatformProvider);
        }

        [Fact]
        public void NormalizeContainerName()
        {
            var filename = "thisismy:*?\"<>|foldername";
            filename = _blobNamingNormalizer.NormalizeContainerName(filename);
            filename.ShouldBe("thisismyfoldername");
        }

        [Fact]
        public void NormalizeBlobName()
        {
            var filename = "thisismy:*?\"<>|filename";
            filename = _blobNamingNormalizer.NormalizeContainerName(filename);
            filename.ShouldBe("thisismyfilename");
        }

        [Theory]
        [InlineData("/")]
        [InlineData("\\")]
        public void NormalizeContainerName_With_Directory(string delimiter)
        {
            var filename = $"thisis{delimiter}my:*?\"<>|{delimiter}foldername";
            filename = _blobNamingNormalizer.NormalizeContainerName(filename);
            filename.ShouldBe($"thisis{delimiter}my{delimiter}foldername");
        }

        [Theory]
        [InlineData("/")]
        [InlineData("\\")]
        public void NormalizeBlobName_With_Directory(string delimiter)
        {
            var filename = $"thisis{delimiter}my:*?\"<>|{delimiter}filename";
            filename = _blobNamingNormalizer.NormalizeContainerName(filename);
            filename.ShouldBe($"thisis{delimiter}my{delimiter}filename");
        }
    }
}
