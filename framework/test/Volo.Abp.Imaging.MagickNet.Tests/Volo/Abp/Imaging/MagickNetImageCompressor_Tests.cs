using System.IO;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.Imaging;

public class MagickNetImageCompressor_Tests : AbpImagingMagickNetTestBase
{
    public IImageCompressor ImageCompressor { get; }
    
    public MagickNetImageCompressor_Tests()
    {
        ImageCompressor = GetRequiredService<IImageCompressor>();
    }
    
    [Fact]
    public async Task Should_Compress_Jpg()
    {
        await using var jpegImage = ImageFileHelper.GetJpgTestFileStream();
        var compressedImage = await ImageCompressor.CompressAsync(jpegImage);

        compressedImage.ShouldNotBeNull();
        compressedImage.State.ShouldBe(ImageProcessState.Done);
        compressedImage.Result.Length.ShouldBeLessThan(jpegImage.Length);
        
        compressedImage.Result.Dispose();
    }

    [Fact]
    public async Task Should_Compress_Png()
    {
        await using var pngImage = ImageFileHelper.GetPngTestFileStream();
        var compressedImage = await ImageCompressor.CompressAsync(pngImage);

        compressedImage.ShouldNotBeNull();
        compressedImage.State.ShouldBe(ImageProcessState.Done);
        compressedImage.Result.Length.ShouldBeLessThan(pngImage.Length);
        
        compressedImage.Result.Dispose();
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
    
    [Fact]
    public async Task Should_Compress_Stream_And_Byte_Array_The_Same()
    {
        await using var jpegImage = ImageFileHelper.GetJpgTestFileStream();
        var compressedImage1 = await ImageCompressor.CompressAsync(jpegImage);
        var compressedImage2 = await ImageCompressor.CompressAsync(await jpegImage.GetAllBytesAsync());

        compressedImage1.ShouldNotBeNull();
        compressedImage1.State.ShouldBe(ImageProcessState.Done);
        compressedImage1.Result.Length.ShouldBeLessThan(jpegImage.Length);
        
        compressedImage2.ShouldNotBeNull();
        compressedImage2.State.ShouldBe(ImageProcessState.Done);
        compressedImage2.Result.LongLength.ShouldBeLessThan(jpegImage.Length);
        
        compressedImage1.Result.Length.ShouldBe(compressedImage2.Result.LongLength);
        
        compressedImage1.Result.Dispose();
    }
}