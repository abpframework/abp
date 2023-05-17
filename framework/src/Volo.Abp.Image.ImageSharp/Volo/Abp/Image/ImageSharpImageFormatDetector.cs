using System.IO;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Image;

public class ImageSharpImageFormatDetector : IImageFormatDetector, ITransientDependency
{
    public IImageFormat FindFormat(Stream stream)
    {
        var format = SixLabors.ImageSharp.Image.DetectFormat(stream);
        return new ImageFormat(format.DefaultMimeType);
    }
}