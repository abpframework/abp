using System.IO;
using System.Threading.Tasks;
using Shouldly;
using SkiaSharp;
using Xunit;

namespace Volo.Abp.Imaging;

public class SkiaSharpImageResizerTests : AbpImagingSkiaSharpTestBase
{
    public IImageResizer ImageResizer { get; }

    public SkiaSharpImageResizerTests()
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

    [Fact]
    public async Task Should_Return_Resized_Stream_Positioned_At_Start()
    {
        await using var jpegImage = ImageFileHelper.GetJpgTestFileStream();
        var resizedImage = await ImageResizer.ResizeAsync(jpegImage, new ImageResizeArgs(100, 100));

        resizedImage.ShouldNotBeNull();
        resizedImage.State.ShouldBe(ImageProcessState.Done);
        resizedImage.Result.Position.ShouldBe(0);

        using var copy = new MemoryStream();
        await resizedImage.Result.CopyToAsync(copy);
        copy.Length.ShouldBe(resizedImage.Result.Length);

        resizedImage.Result.Dispose();
    }

    [Fact]
    public async Task Should_Return_Unsupported_For_Gif_Stream()
    {
        await using var gifImage = ImageFileHelper.GetGifTestFileStream();
        var resizedImage = await ImageResizer.ResizeAsync(gifImage, new ImageResizeArgs(100, 100));

        resizedImage.ShouldNotBeNull();
        resizedImage.State.ShouldBe(ImageProcessState.Unsupported);
        resizedImage.Result.ShouldBe(gifImage);
    }

    [Fact]
    public async Task Should_Return_Unsupported_For_Gif_Bytes()
    {
        await using var gifImage = ImageFileHelper.GetGifTestFileStream();
        var bytes = await gifImage.GetAllBytesAsync();
        var resizedImage = await ImageResizer.ResizeAsync(bytes, new ImageResizeArgs(100, 100));

        resizedImage.ShouldNotBeNull();
        resizedImage.State.ShouldBe(ImageProcessState.Unsupported);
        resizedImage.Result.ShouldBe(bytes);
    }

    [Theory]
    [InlineData(ImageResizeMode.None)]
    [InlineData(ImageResizeMode.Stretch)]
    [InlineData(ImageResizeMode.Crop)]
    [InlineData(ImageResizeMode.Pad)]
    [InlineData(ImageResizeMode.BoxPad)]
    [InlineData(ImageResizeMode.Default)]
    public async Task Should_Produce_Exact_Target_Size_For_Fixed_Modes(ImageResizeMode mode)
    {
        await using var jpegImage = ImageFileHelper.GetJpgTestFileStream();
        var resizedImage = await ImageResizer.ResizeAsync(jpegImage, new ImageResizeArgs(120, 80, mode));

        resizedImage.State.ShouldBe(ImageProcessState.Done);
        using var decoded = SKBitmap.Decode(resizedImage.Result);
        decoded.ShouldNotBeNull();
        decoded.Width.ShouldBe(120);
        decoded.Height.ShouldBe(80);
        resizedImage.Result.Dispose();
    }

    [Fact]
    public async Task Should_Produce_Bounded_Size_For_Max_Mode()
    {
        await using var jpegImage = ImageFileHelper.GetJpgTestFileStream();
        var resizedImage = await ImageResizer.ResizeAsync(jpegImage, new ImageResizeArgs(120, 80, ImageResizeMode.Max));

        resizedImage.State.ShouldBe(ImageProcessState.Done);
        using var decoded = SKBitmap.Decode(resizedImage.Result);
        decoded.Width.ShouldBeLessThanOrEqualTo(120);
        decoded.Height.ShouldBeLessThanOrEqualTo(80);
        (decoded.Width == 120 || decoded.Height == 80).ShouldBeTrue();
        resizedImage.Result.Dispose();
    }

    [Fact]
    public async Task Should_Max_Fit_Source_Larger_Than_Target_For_BoxPad_Mode()
    {
        using var src = new SKBitmap(400, 100);
        using (var canvas = new SKCanvas(src))
        {
            canvas.Clear(SKColors.Red);
        }
        using var srcImage = SKImage.FromBitmap(src);
        using var srcData = srcImage.Encode(SKEncodedImageFormat.Png, 100);
        using var srcStream = new MemoryStream();
        srcData.SaveTo(srcStream);
        srcStream.Position = 0;

        var resizedImage = await ImageResizer.ResizeAsync(srcStream, new ImageResizeArgs(100, 100, ImageResizeMode.BoxPad));

        resizedImage.State.ShouldBe(ImageProcessState.Done);
        using var decoded = SKBitmap.Decode(resizedImage.Result);
        decoded.Width.ShouldBe(100);
        decoded.Height.ShouldBe(100);

        decoded.GetPixel(50, 0).Alpha.ShouldBe((byte)0);
        decoded.GetPixel(50, 99).Alpha.ShouldBe((byte)0);
        decoded.GetPixel(50, 50).Red.ShouldBeGreaterThan((byte)200);

        resizedImage.Result.Dispose();
    }

    [Fact]
    public async Task Should_Produce_Bounded_Size_For_Min_Mode()
    {
        await using var jpegImage = ImageFileHelper.GetJpgTestFileStream();
        var resizedImage = await ImageResizer.ResizeAsync(jpegImage, new ImageResizeArgs(120, 80, ImageResizeMode.Min));

        resizedImage.State.ShouldBe(ImageProcessState.Done);
        using var decoded = SKBitmap.Decode(resizedImage.Result);
        decoded.Width.ShouldBeGreaterThanOrEqualTo(120);
        decoded.Height.ShouldBeGreaterThanOrEqualTo(80);
        (decoded.Width == 120 || decoded.Height == 80).ShouldBeTrue();
        resizedImage.Result.Dispose();
    }
}
