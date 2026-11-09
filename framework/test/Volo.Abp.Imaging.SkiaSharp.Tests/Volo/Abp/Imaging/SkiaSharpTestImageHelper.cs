using System.IO;
using SkiaSharp;

namespace Volo.Abp.Imaging;

public static class SkiaSharpTestImageHelper
{
    public static MemoryStream CreateQuadrantJpegWithExifOrientation(ushort orientation)
    {
        using var bitmap = new SKBitmap(200, 100);
        using (var canvas = new SKCanvas(bitmap))
        {
            using var paint = new SKPaint();
            paint.Color = SKColors.Red;
            canvas.DrawRect(0, 0, 100, 50, paint);
            paint.Color = SKColors.Lime;
            canvas.DrawRect(100, 0, 100, 50, paint);
            paint.Color = SKColors.Blue;
            canvas.DrawRect(0, 50, 100, 50, paint);
            paint.Color = SKColors.Yellow;
            canvas.DrawRect(100, 50, 100, 50, paint);
        }

        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Jpeg, 100);
        var jpeg = data.ToArray();

        var exif = new byte[]
        {
            0xFF, 0xE1, 0x00, 0x22,
            (byte)'E', (byte)'x', (byte)'i', (byte)'f', 0x00, 0x00,
            (byte)'M', (byte)'M', 0x00, 0x2A, 0x00, 0x00, 0x00, 0x08,
            0x00, 0x01,
            0x01, 0x12, 0x00, 0x03, 0x00, 0x00, 0x00, 0x01, (byte)(orientation >> 8), (byte)orientation, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00
        };

        var stream = new MemoryStream();
        stream.Write(jpeg, 0, 2);
        stream.Write(exif, 0, exif.Length);
        stream.Write(jpeg, 2, jpeg.Length - 2);
        stream.Position = 0;
        return stream;
    }

    public static string GetQuadrantColors(SKBitmap bitmap)
    {
        return GetColorInitial(bitmap.GetPixel(bitmap.Width / 4, bitmap.Height / 4)) +
               GetColorInitial(bitmap.GetPixel(bitmap.Width * 3 / 4, bitmap.Height / 4)) +
               GetColorInitial(bitmap.GetPixel(bitmap.Width / 4, bitmap.Height * 3 / 4)) +
               GetColorInitial(bitmap.GetPixel(bitmap.Width * 3 / 4, bitmap.Height * 3 / 4));
    }

    private static string GetColorInitial(SKColor color)
    {
        var red = color.Red > 200;
        var green = color.Green > 200;
        var blue = color.Blue > 200;

        return (red, green, blue) switch
        {
            (true, true, false) => "Y",
            (true, false, false) => "R",
            (false, true, false) => "G",
            (false, false, true) => "B",
            _ => "?"
        };
    }
}
