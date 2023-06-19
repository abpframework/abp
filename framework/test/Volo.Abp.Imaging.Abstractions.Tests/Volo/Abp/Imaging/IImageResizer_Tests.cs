using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.Imaging;

public class IImageResizer_Tests : AbpImagingAbstractionsTestBase
{
    protected IImageResizer ImageResizer { get; }

    public IImageResizer_Tests()
    {
        ImageResizer = GetRequiredService<IImageResizer>();
    }
    [Fact]
    public async Task Should_Resize_Jpg()
    {
        await using var jpegImage = ImageFileHelper.GetJpgTestFileStream();
        var resizedImage = await ImageResizer.ResizeAsync(jpegImage, new ImageResizeArgs(100, 100));
        
        resizedImage.ShouldNotBeNull();
        resizedImage.State.ShouldBe(ImageProcessState.Unsupported);
        
        resizedImage.Result.ShouldBe(jpegImage);
    }
    
    [Fact]
    public async Task Should_Resize_Png()
    {
        await using var pngImage = ImageFileHelper.GetPngTestFileStream();
        var resizedImage = await ImageResizer.ResizeAsync(pngImage, new ImageResizeArgs(100, 100));
        
        resizedImage.ShouldNotBeNull();
        resizedImage.State.ShouldBe(ImageProcessState.Unsupported);
        
        resizedImage.Result.ShouldBe(pngImage);
    }
    
    [Fact]
    public async Task Should_Resize_Webp()
    {
        await using var webpImage = ImageFileHelper.GetWebpTestFileStream();
        var resizedImage = await ImageResizer.ResizeAsync(webpImage, new ImageResizeArgs(100, 100));
        
        resizedImage.ShouldNotBeNull();
        resizedImage.State.ShouldBe(ImageProcessState.Unsupported);
        
        resizedImage.Result.ShouldBe(webpImage);
    }
}