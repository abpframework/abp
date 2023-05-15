using System.IO;
using ImageMagick;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Image;

public class MagickImageFormatDetector : IImageFormatDetector, ITransientDependency
{
    public IImageFormat FindFormat(Stream image)
    {
        using var magickImage = new MagickImage(image);
        var format = magickImage.FormatInfo;
        return format == null ? null : new ImageFormat(format.Format.ToString(), format.MimeType ?? string.Empty);
    }
}