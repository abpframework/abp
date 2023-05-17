namespace Volo.Abp.Image;

public interface IImageCompressorSelector //TODO: Remove, merge to IImageCompressorManager
{
    IImageCompressor FindCompressor(IImageFormat imageFormat);
}