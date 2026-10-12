using System;
using System.IO;
using Shouldly;
using SkiaSharp;
using Volo.Blogging.Areas.Blog.Helpers;
using Volo.Blogging.Files;
using Xunit;

namespace Volo.Blogging
{
    public class ImageFormatHelper_Tests
    {
        [Theory]
        [InlineData(SKEncodedImageFormat.Png, true)]
        [InlineData(SKEncodedImageFormat.Jpeg, true)]
        [InlineData(SKEncodedImageFormat.Webp, false)]
        public void Should_Check_Image_From_Current_Stream_Position(SKEncodedImageFormat format, bool expected)
        {
            var image = CreateImage(format);
            using var stream = new MemoryStream();
            stream.Write(new byte[16]);
            stream.Write(image);
            stream.Position = 16;

            ImageFormatHelper.IsValidImage(stream, FileUploadConsts.AllowedImageUploadFormats).ShouldBe(expected);

            stream.CanRead.ShouldBeTrue();
        }

        [Theory]
        [InlineData(SKEncodedImageFormat.Png, true)]
        [InlineData(SKEncodedImageFormat.Jpeg, true)]
        [InlineData(SKEncodedImageFormat.Webp, false)]
        public void Should_Check_Image_From_Non_Seekable_Stream(SKEncodedImageFormat format, bool expected)
        {
            using var stream = new NonSeekableStream(CreateImage(format));

            ImageFormatHelper.IsValidImage(stream, FileUploadConsts.AllowedImageUploadFormats).ShouldBe(expected);

            stream.CanRead.ShouldBeTrue();
        }

        [Fact]
        public void Should_Return_Null_For_Non_Image_Content()
        {
            using var stream = new MemoryStream(new byte[] { 1, 2, 3, 4 });

            ImageFormatHelper.GetImageRawFormat(stream).ShouldBeNull();
        }

        [Fact]
        public void Should_Return_Null_For_Empty_Stream()
        {
            using var stream = new MemoryStream();

            ImageFormatHelper.GetImageRawFormat(stream).ShouldBeNull();
        }

        [Fact]
        public void Should_Not_Be_Valid_When_Reading_Stream_Fails()
        {
            using var stream = new NonSeekableStream(CreateImage(SKEncodedImageFormat.Png), new IOException());

            ImageFormatHelper.IsValidImage(stream, FileUploadConsts.AllowedImageUploadFormats).ShouldBeFalse();

            stream.CanRead.ShouldBeTrue();
        }

        [Fact]
        public void Should_Not_Hide_Native_Library_Loading_Errors()
        {
            using var stream = new NonSeekableStream(CreateImage(SKEncodedImageFormat.Png), new DllNotFoundException());

            Should.Throw<DllNotFoundException>(() => ImageFormatHelper.IsValidImage(stream, FileUploadConsts.AllowedImageUploadFormats));
        }

        private static byte[] CreateImage(SKEncodedImageFormat format)
        {
            using (var bitmap = new SKBitmap(1, 1))
            using (var image = SKImage.FromBitmap(bitmap))
            using (var data = image.Encode(format, 100))
            {
                return data.ToArray();
            }
        }

        private class NonSeekableStream : Stream
        {
            private readonly MemoryStream _innerStream;
            private readonly Exception _readException;

            public NonSeekableStream(byte[] content, Exception readException = null)
            {
                _innerStream = new MemoryStream(content);
                _readException = readException;
            }

            public override bool CanRead => _innerStream.CanRead;

            public override bool CanSeek => false;

            public override bool CanWrite => false;

            public override long Length => throw new NotSupportedException();

            public override long Position
            {
                get => throw new NotSupportedException();
                set => throw new NotSupportedException();
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                if (_readException != null && _innerStream.Position > 0)
                {
                    throw _readException;
                }

                return _innerStream.Read(buffer, offset, Math.Min(count, 7));
            }

            public override void Flush()
            {
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    _innerStream.Dispose();
                }

                base.Dispose(disposing);
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                throw new NotSupportedException();
            }

            public override void SetLength(long value)
            {
                throw new NotSupportedException();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                throw new NotSupportedException();
            }
        }
    }
}
