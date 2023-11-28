using System;
using System.Security.Cryptography;
using System.Text;
using ImageMagick;

namespace Volo.CmsKit.Public.Web.Security.Captcha;
public static class RandomTextGenerator
{
    private static readonly char[] AllowedChars = "abcdefghijkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVXYZW23456789".ToCharArray();

    public static MagickFormat GetEncoder(EncoderTypes encoderType)
    {
        var encoder = encoderType switch
        {
            EncoderTypes.Png => MagickFormat.Png,
            EncoderTypes.Jpeg => MagickFormat.Jpeg,
            _ => throw new ArgumentException($"Encoder '{encoderType}' not found!")
        };

        return encoder;
    }

    public static string GetRandomText(int size)
    {
        var data = new byte[4 * size];
        using (var crypto = new RNGCryptoServiceProvider())
        {
            crypto.GetBytes(data);
        }

        var result = new StringBuilder(size);
        for (var i = 0; i < size; i++)
        {
            var rnd = BitConverter.ToUInt32(data, i * 4);
            var idx = rnd % AllowedChars.Length;
            result.Append(AllowedChars[idx]);
        }

        return result.ToString();
    }

    public static string GetUniqueKey(int size, char[] chars)
    {
        var data = new byte[4 * size];
        using (var crypto = new RNGCryptoServiceProvider())
        {
            crypto.GetBytes(data);
        }

        var result = new StringBuilder(size);

        for (var i = 0; i < size; i++)
        {
            var rnd = BitConverter.ToUInt32(data, i * 4);
            var idx = rnd % chars.Length;
            result.Append(chars[idx]);
        }

        return result.ToString();
    }

    public static float GenerateNextFloat(double min = -3.40282347E+38, double max = 3.40282347E+38)
    {
        var random = new Random();
        var range = max - min;
        var sample = random.NextDouble();
        var scaled = sample * range + min;
        var result = (float)scaled;
        return result;
    }
}
