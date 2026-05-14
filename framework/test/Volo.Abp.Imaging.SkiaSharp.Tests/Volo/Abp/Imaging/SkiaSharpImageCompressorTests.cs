using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Volo.Abp.Imaging;

public class SkiaSharpImageCompressorTests : AbpImagingSkiaSharpTestBase
{
    public IImageCompressor ImageCompressor { get; }

    public SkiaSharpImageCompressorTests()
    {
        ImageCompressor = GetRequiredService<IImageCompressor>();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.Configure<SkiaSharpCompressOptions>(options =>
        {
            options.Quality = 50;
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
        }
        else
        {
            compressedImage.State.ShouldBe(ImageProcessState.Canceled);
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

    [Fact]
    public async Task Should_Return_Compressed_Stream_Positioned_At_Start()
    {
        await using var jpegImage = ImageFileHelper.GetJpgTestFileStream();
        var compressedImage = await ImageCompressor.CompressAsync(jpegImage);

        compressedImage.ShouldNotBeNull();
        compressedImage.State.ShouldBe(ImageProcessState.Done);
        compressedImage.Result.Position.ShouldBe(0);

        using var copy = new MemoryStream();
        await compressedImage.Result.CopyToAsync(copy);
        copy.Length.ShouldBe(compressedImage.Result.Length);

        compressedImage.Result.Dispose();
    }

    [Fact]
    public async Task Should_Return_Unsupported_For_Gif_Stream()
    {
        await using var gifImage = ImageFileHelper.GetGifTestFileStream();
        var compressedImage = await ImageCompressor.CompressAsync(gifImage);

        compressedImage.ShouldNotBeNull();
        compressedImage.State.ShouldBe(ImageProcessState.Unsupported);
        compressedImage.Result.ShouldBe(gifImage);
    }

    [Fact]
    public async Task Should_Return_Unsupported_For_Gif_Bytes()
    {
        await using var gifImage = ImageFileHelper.GetGifTestFileStream();
        var bytes = await gifImage.GetAllBytesAsync();
        var compressedImage = await ImageCompressor.CompressAsync(bytes);

        compressedImage.ShouldNotBeNull();
        compressedImage.State.ShouldBe(ImageProcessState.Unsupported);
        compressedImage.Result.ShouldBe(bytes);
    }

    [Fact]
    public async Task Should_Handle_Non_Seekable_Stream_Directly_On_Contributor()
    {
        var contributor = GetRequiredService<IImageCompressorContributor>();

        await using var jpegImage = ImageFileHelper.GetJpgTestFileStream();
        var bytes = await jpegImage.GetAllBytesAsync();
        await using var nonSeekable = new NonSeekableStream(new MemoryStream(bytes));

        var compressedImage = await contributor.TryCompressAsync(nonSeekable, "image/jpeg");

        compressedImage.ShouldNotBeNull();
        compressedImage.State.ShouldBeOneOf(ImageProcessState.Done, ImageProcessState.Canceled);
        if (compressedImage.State == ImageProcessState.Done)
        {
            compressedImage.Result.ShouldNotBe(nonSeekable);
            compressedImage.Result.Dispose();
        }
    }

    private sealed class NonSeekableStream : Stream
    {
        private readonly Stream _inner;
        public NonSeekableStream(Stream inner) { _inner = inner; }
        public override bool CanRead => _inner.CanRead;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => throw new NotSupportedException();
        public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }
        public override int Read(byte[] buffer, int offset, int count) => _inner.Read(buffer, offset, count);
        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) => _inner.ReadAsync(buffer, offset, count, cancellationToken);
        public override void Flush() { }
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
        protected override void Dispose(bool disposing)
        {
            if (disposing) _inner.Dispose();
            base.Dispose(disposing);
        }
    }
}
