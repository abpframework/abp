namespace Volo.Abp.Image;

public interface IImageCompressorSelector
{
    IImageCompressor FindCompressor(IImageFormat imageFormat);
}