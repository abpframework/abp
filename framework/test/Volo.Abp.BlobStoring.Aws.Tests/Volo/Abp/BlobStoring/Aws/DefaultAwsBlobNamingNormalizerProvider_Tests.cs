using Shouldly;
using Xunit;

namespace Volo.Abp.BlobStoring.Aws
{
    public class DefaultAwsBlobNamingNormalizerProviderTests : AbpBlobStoringAwsTestCommonBase
    {
        private readonly IBlobNamingNormalizer _blobNamingNormalizer;

        public DefaultAwsBlobNamingNormalizerProviderTests()
        {
            _blobNamingNormalizer = GetRequiredService<IBlobNamingNormalizer>();
        }

        [Fact]
        public void NormalizeContainerName_Lowercase()
        {
            var filename = "ThisIsMyContainerName";
            filename = _blobNamingNormalizer.NormalizeContainerName(filename);
            filename.ShouldBe("thisismycontainername");
        }

        [Fact]
        public void NormalizeContainerName_Only_Letters_Numbers_Dash_Dots()
        {
            var filename = ",./this-i,/s-my-c,/ont,/ai+*/=!@#$n^&*er.name+/";
            filename = _blobNamingNormalizer.NormalizeContainerName(filename);
            filename.ShouldBe("this-is-my-container.name");
        }

        [Fact]
        public void NormalizeContainerName_Dash()
        {
            var filename = "-this.--is-.-.-my--container---name-";
            filename = _blobNamingNormalizer.NormalizeContainerName(filename);
            filename.ShouldBe("this-is-my--container---name");
        }

        [Fact]
        public void NormalizeContainerName_Min_Length()
        {
            var filename = "a";
            filename = _blobNamingNormalizer.NormalizeContainerName(filename);
            filename.Length.ShouldBeGreaterThanOrEqualTo(3);
        }

        [Fact]
        public void NormalizeContainerName_Max_Length()
        {
            var filename = "abpabpabpabpabpabpabpabpabpabpabpabpabpabpabpabpabpabpabpabpabpabpabp";
            filename = _blobNamingNormalizer.NormalizeContainerName(filename);
            filename.Length.ShouldBeLessThanOrEqualTo(63);
        }

        [Fact]
        public void NormalizeContainerName_Must_Not_Be_Ip_Address()
        {
            var filename = "192.168.5.4";
            filename = _blobNamingNormalizer.NormalizeContainerName(filename);
            filename.ShouldBe("000");

            filename = "a.192.168.5.4";
            filename = _blobNamingNormalizer.NormalizeContainerName(filename);
            filename.ShouldBe("a.192.168.5.4");
        }

        [Fact]
        public void NormalizeContainerName_Dots()
        {
            var filename = ".this..is.-.my.container....name.";
            filename = _blobNamingNormalizer.NormalizeContainerName(filename);
            filename.ShouldBe("this.is.my.container.name");
        }
    }
}
