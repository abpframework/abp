using Shouldly;
using Xunit;

namespace Volo.Abp.BlobStoring.Google;

public class DefaultGoogleBlobNamingNormalizerProvider_Tests: AbpBlobStoringGoogleTestCommonBase
{
    private readonly IBlobNamingNormalizer _blobNamingNormalizer;

    public DefaultGoogleBlobNamingNormalizerProvider_Tests()
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
    public void NormalizeContainerName_Only_Letters_Numbers_Dash_Dots_Underscores()
    {
        var filename = ",./this-i,/s-my-c,/ont,/ai+*/=!@#$n^&*er.name+/";
        filename = _blobNamingNormalizer.NormalizeContainerName(filename);
        filename.ShouldBe("this-is-my-container.name");
    }

    [Fact]
    public void NormalizeContainerName_Only_Start_With_Letters_Or_Numbers()
    {
        var filename = "-this.--is-.-.-my--_container---name-";
        filename = _blobNamingNormalizer.NormalizeContainerName(filename);
        filename.ShouldBe("this.--is-.-.-my--_container---name");
        
        filename = ".this.--is-.-.-my--container---name0._";
        filename = _blobNamingNormalizer.NormalizeContainerName(filename);
        filename.ShouldBe("this.--is-.-.-my--container---name0");
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
        var filename = ".this..is.my.container....name.";
        filename = _blobNamingNormalizer.NormalizeContainerName(filename);
        filename.ShouldBe("this.is.my.container.name");
    }
    
    [Fact]
    public void NormalizeContainerName_DNS()
    {
        var filename = "bucket...example..com";
        filename = _blobNamingNormalizer.NormalizeContainerName(filename);
        filename.ShouldBe("bucket.example.com");
    }

    [Fact]
    public void NormalizeContainerName_Max_Length_Dash()
    {
        var filename = "-this-is-my-container-name-abpabpabpabpabpabpabpabp-a-b-p-a--b-p-";
        filename = _blobNamingNormalizer.NormalizeContainerName(filename);
        filename.ShouldBe("this-is-my-container-name-abpabpabpabpabpabpabpabp-a-b-p-a--b");
    }
}
