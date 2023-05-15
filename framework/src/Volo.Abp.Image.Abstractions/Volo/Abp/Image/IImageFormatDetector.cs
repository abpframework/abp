using System.IO;

namespace Volo.Abp.Image;

public interface IImageFormatDetector
{
    IImageFormat FindFormat(Stream image);
}