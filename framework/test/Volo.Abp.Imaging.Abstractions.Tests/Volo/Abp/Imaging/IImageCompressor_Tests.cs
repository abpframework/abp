using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.Imaging;

public class IImageCompressor_Tests : AbpImagingAbstractionsTestBase
{
    protected IImageCompressor ImageCompressor { get; }

    public IImageCompressor_Tests()
    {
        ImageCompressor = GetRequiredService<IImageCompressor>();
    }
    
    [Fact]
    public async Task Should_Compress_Jpg()
    {
        await using var jpegImage = ImageFileHelper.GetJpgTestFileStream();
        var compressedImage = await ImageCompressor.CompressAsync(jpegImage);
        
        compressedImage.ShouldNotBeNull();
        compressedImage.State.ShouldBe(ImageProcessState.Unsupported);
        
        compressedImage.Result.ShouldBe(jpegImage);
    }
    
    [Fact]
    public async Task Should_Compress_Png()
    {
        await using var pngImage = ImageFileHelper.GetPngTestFileStream();
        var compressedImage = await ImageCompressor.CompressAsync(pngImage);
        
        compressedImage.ShouldNotBeNull();
        compressedImage.State.ShouldBe(ImageProcessState.Unsupported);
        
        compressedImage.Result.ShouldBe(pngImage);
    }
    
    [Fact]
    public async Task Should_Compress_Webp()
    {
        await using var webpImage = ImageFileHelper.GetWebpTestFileStream();
        var compressedImage = await ImageCompressor.CompressAsync(webpImage);
        
        compressedImage.ShouldNotBeNull();
        compressedImage.State.ShouldBe(ImageProcessState.Unsupported);
        
        compressedImage.Result.ShouldBe(webpImage);
    }
}