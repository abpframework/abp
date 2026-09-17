using Shouldly;
using Xunit;

namespace Volo.Abp.BlobStoring.Bunny;

public class DefaultBunnyBlobNamingNormalizerProviderTests : AbpBlobStoringBunnyTestCommonBase
{
    private readonly IBlobNamingNormalizer _blobNamingNormalizer;

    public DefaultBunnyBlobNamingNormalizerProviderTests()
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
        filename.ShouldBe("this-is-my-containername");
    }

    [Fact]
    public void NormalizeContainerName_Min_Length()
    {
        var filename = "a";
        Assert.Throws<AbpException>(()=>
        {
            filename = _blobNamingNormalizer.NormalizeContainerName(filename);
        });
    }

    [Fact]
    public void NormalizeContainerName_Max_Length()
    {
        var longName = new string('a', 65); // 65 characters
        var exception = Assert.Throws<AbpException>(() =>
            _blobNamingNormalizer.NormalizeContainerName(longName)
        );
    }

    [Fact]
    public void NormalizeContainerName_Dots()
    {
        var filename = ".this..is.-.my.container....name.";
        filename = _blobNamingNormalizer.NormalizeContainerName(filename);
        filename.ShouldBe("thisis-mycontainername");
    }
}
