using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Webp;
using Xunit;

namespace Volo.Abp.Imaging;

public class ImageSharpImageCompressor_Tests : AbpImagingImageSharpTestBase
{
    public IImageCompressor ImageCompressor { get; }

    public ImageSharpImageCompressor_Tests()
    {
        ImageCompressor = GetRequiredService<IImageCompressor>();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.Configure<ImageSharpCompressOptions>(options =>
        {
            options.JpegEncoder = new JpegEncoder
            {
                Quality = 50
            };
            options.WebpEncoder = new WebpEncoder
            {
                Quality = 50
            };
        });

        base.AfterAddApplication(services);
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

        if (compressedImage.State == ImageProcessState.Done)
        {
            compressedImage.Result.Length.ShouldBeLessThan(pngImage.Length);
        }else
        {
            compressedImage.Result.Length.ShouldBe(pngImage.Length);
        }
        compressedImage.Result.Dispose();
    }

    [Fact]
    public async Task Should_Compress_Webp()
    {
        await using var webpImage = ImageFileHelper.GetWebpTestFileStream();
        var compressedImage = await ImageCompressor.CompressAsync(webpImage);

        compressedImage.ShouldNotBeNull();
        compressedImage.State.ShouldBe(ImageProcessState.Done);
        compressedImage.Result.Length.ShouldBeLessThan(webpImage.Length);
        compressedImage.Result.Dispose();
    }

    [Fact]
    public async Task Should_Compress_Stream_And_Byte_Array_The_Same()
    {
        await using var jpegImage = ImageFileHelper.GetJpgTestFileStream();
        var byteArr = await jpegImage.GetAllBytesAsync();

        var compressedImage1 = await ImageCompressor.CompressAsync(jpegImage);
        var compressedImage2 = await ImageCompressor.CompressAsync(byteArr);

        compressedImage1.ShouldNotBeNull();
        compressedImage1.State.ShouldBe(ImageProcessState.Done);

        compressedImage2.ShouldNotBeNull();
        compressedImage2.State.ShouldBe(ImageProcessState.Done);

        compressedImage1.Result.Length.ShouldBeLessThan(jpegImage.Length);
        compressedImage2.Result.LongLength.ShouldBeLessThan(jpegImage.Length);

        compressedImage1.Result.Length.ShouldBe(compressedImage2.Result.LongLength);

        compressedImage1.Result.Dispose();
    }
}
