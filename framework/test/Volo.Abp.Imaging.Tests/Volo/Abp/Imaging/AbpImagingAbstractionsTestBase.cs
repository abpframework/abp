using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Imaging;

public abstract class AbpImagingAbstractionsTestBase : AbpIntegratedTest<AbpImagingAbstractionsTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}

public class IIImageResizer_Tests : AbpImagingAbstractionsTestBase
{
    protected IImageResizer ImageResizer { get; }

    public IIImageResizer_Tests()
    {
        ImageResizer = GetRequiredService<IImageResizer>();
    }
    [Fact]
    public async Task Should_Resize_Jpg()
    {
        await using var jpegImage = ImageFileHelper.GetJpgTestFileStream();
        var resizedImage = await ImageResizer.ResizeAsync(jpegImage, new ImageResizeArgs(100, 100));
        
        resizedImage.ShouldNotBeNull();
        resizedImage.State.ShouldBe(ProcessState.Unsupported);
        
        resizedImage.Result.ShouldBe(jpegImage);
    }
    
    [Fact]
    public async Task Should_Resize_Png()
    {
        await using var pngImage = ImageFileHelper.GetPngTestFileStream();
        var resizedImage = await ImageResizer.ResizeAsync(pngImage, new ImageResizeArgs(100, 100));
        
        resizedImage.ShouldNotBeNull();
        resizedImage.State.ShouldBe(ProcessState.Unsupported);
        
        resizedImage.Result.ShouldBe(pngImage);
    }
    
    [Fact]
    public async Task Should_Resize_Webp()
    {
        await using var webpImage = ImageFileHelper.GetWebpTestFileStream();
        var resizedImage = await ImageResizer.ResizeAsync(webpImage, new ImageResizeArgs(100, 100));
        
        resizedImage.ShouldNotBeNull();
        resizedImage.State.ShouldBe(ProcessState.Unsupported);
        
        resizedImage.Result.ShouldBe(webpImage);
    }
}

public class IIImageCompressor_Tests : AbpImagingAbstractionsTestBase
{
    protected IImageCompressor ImageCompressor { get; }

    public IIImageCompressor_Tests()
    {
        ImageCompressor = GetRequiredService<IImageCompressor>();
    }
    
    [Fact]
    public async Task Should_Compress_Jpg()
    {
        await using var jpegImage = ImageFileHelper.GetJpgTestFileStream();
        var compressedImage = await ImageCompressor.CompressAsync(jpegImage);
        
        compressedImage.ShouldNotBeNull();
        compressedImage.State.ShouldBe(ProcessState.Unsupported);
        
        compressedImage.Result.ShouldBe(jpegImage);
    }
    
    [Fact]
    public async Task Should_Compress_Png()
    {
        await using var pngImage = ImageFileHelper.GetPngTestFileStream();
        var compressedImage = await ImageCompressor.CompressAsync(pngImage);
        
        compressedImage.ShouldNotBeNull();
        compressedImage.State.ShouldBe(ProcessState.Unsupported);
        
        compressedImage.Result.ShouldBe(pngImage);
    }
    
    [Fact]
    public async Task Should_Compress_Webp()
    {
        await using var webpImage = ImageFileHelper.GetWebpTestFileStream();
        var compressedImage = await ImageCompressor.CompressAsync(webpImage);
        
        compressedImage.ShouldNotBeNull();
        compressedImage.State.ShouldBe(ProcessState.Unsupported);
        
        compressedImage.Result.ShouldBe(webpImage);
    }
}