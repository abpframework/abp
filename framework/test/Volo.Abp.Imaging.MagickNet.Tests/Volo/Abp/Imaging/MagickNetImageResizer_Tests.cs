using System.IO;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.Imaging;

public class MagickNetImageResizer_Tests : AbpImagingMagickNetTestBase
{
    public IImageResizer ImageResizer { get; }
    
    public MagickNetImageResizer_Tests()
    {
        ImageResizer = GetRequiredService<IImageResizer>();
    }
    
    [Fact]
    public async Task Should_Resize_Jpg()
    {
        await using var jpegImage = ImageFileHelper.GetJpgTestFileStream();
        var resizedImage = await ImageResizer.ResizeAsync(jpegImage, new ImageResizeArgs(100, 100));

        resizedImage.ShouldNotBeNull();
        resizedImage.State.ShouldBe(ImageProcessState.Done);
        resizedImage.Result.Length.ShouldBeLessThan(jpegImage.Length);
        
        resizedImage.Result.Dispose();
    }

    [Fact]
    public async Task Should_Resize_Png()
    {
        await using var pngImage = ImageFileHelper.GetPngTestFileStream();
        var resizedImage = await ImageResizer.ResizeAsync(pngImage, new ImageResizeArgs(100, 100));

        resizedImage.ShouldNotBeNull();
        resizedImage.State.ShouldBe(ImageProcessState.Done);
        resizedImage.Result.Length.ShouldBeLessThan(pngImage.Length);
        
        resizedImage.Result.Dispose();
    }

    [Fact]
    public async Task Should_Resize_Webp()
    {
        await using var webpImage = ImageFileHelper.GetWebpTestFileStream();
        var resizedImage = await ImageResizer.ResizeAsync(webpImage, new ImageResizeArgs(100, 100));

        resizedImage.ShouldNotBeNull();
        resizedImage.State.ShouldBe(ImageProcessState.Done);
        resizedImage.Result.Length.ShouldBeLessThan(webpImage.Length);
        
        resizedImage.Result.Dispose();
    }
    
    [Fact]
    public async Task Should_Resize_Stream_And_Byte_Array_The_Same()
    {
        await using var jpegImage = ImageFileHelper.GetJpgTestFileStream();
        var resizedImage1 = await ImageResizer.ResizeAsync(jpegImage, new ImageResizeArgs(100, 100));
        var resizedImage2 = await ImageResizer.ResizeAsync(await jpegImage.GetAllBytesAsync(), new ImageResizeArgs(100, 100));

        resizedImage1.ShouldNotBeNull();
        resizedImage1.State.ShouldBe(ImageProcessState.Done);
        resizedImage1.Result.Length.ShouldBeLessThan(jpegImage.Length);
        
        resizedImage2.ShouldNotBeNull();
        resizedImage2.State.ShouldBe(ImageProcessState.Done);
        resizedImage2.Result.LongLength.ShouldBeLessThan(jpegImage.Length);
        
        resizedImage1.Result.Length.ShouldBe(resizedImage2.Result.LongLength);
        
        resizedImage1.Result.Dispose();
    }
}