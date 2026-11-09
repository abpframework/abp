using SkiaSharp;

namespace Volo.Abp.Imaging;

public static class SkiaSharpBitmapHelper
{
    // The encoders write no EXIF, so the orientation has to be applied to the pixels.
    public static SKBitmap? DecodeUpright(SKCodec codec)
    {
        var bitmap = SKBitmap.Decode(codec);
        if (bitmap == null || codec.EncodedOrigin == SKEncodedOrigin.TopLeft)
        {
            return bitmap;
        }

        using (bitmap)
        {
            return ApplyOrigin(bitmap, codec.EncodedOrigin);
        }
    }

    private static SKBitmap ApplyOrigin(SKBitmap source, SKEncodedOrigin origin)
    {
        var matrix = origin switch
        {
            SKEncodedOrigin.TopRight => new SKMatrix(-1, 0, source.Width, 0, 1, 0, 0, 0, 1),
            SKEncodedOrigin.BottomRight => new SKMatrix(-1, 0, source.Width, 0, -1, source.Height, 0, 0, 1),
            SKEncodedOrigin.BottomLeft => new SKMatrix(1, 0, 0, 0, -1, source.Height, 0, 0, 1),
            SKEncodedOrigin.LeftTop => new SKMatrix(0, 1, 0, 1, 0, 0, 0, 0, 1),
            SKEncodedOrigin.RightTop => new SKMatrix(0, -1, source.Height, 1, 0, 0, 0, 0, 1),
            SKEncodedOrigin.RightBottom => new SKMatrix(0, -1, source.Height, -1, 0, source.Width, 0, 0, 1),
            SKEncodedOrigin.LeftBottom => new SKMatrix(0, 1, 0, -1, 0, source.Width, 0, 0, 1),
            _ => SKMatrix.Identity
        };

        var swapSides = origin is SKEncodedOrigin.LeftTop or SKEncodedOrigin.RightTop or SKEncodedOrigin.RightBottom or SKEncodedOrigin.LeftBottom;
        var info = swapSides ? source.Info.WithSize(source.Height, source.Width) : source.Info;

        // DrawBitmap copies the pixels of a mutable bitmap, which doubles the memory for a large photo.
        source.SetImmutable();

        var result = new SKBitmap(info);
        try
        {
            using var canvas = new SKCanvas(result);
            canvas.SetMatrix(matrix);
            canvas.DrawBitmap(source, 0, 0);
            return result;
        }
        catch
        {
            result.Dispose();
            throw;
        }
    }
}
