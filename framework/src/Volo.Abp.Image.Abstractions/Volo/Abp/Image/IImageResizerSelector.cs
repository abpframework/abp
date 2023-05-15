namespace Volo.Abp.Image;

public interface IImageResizerSelector
{
    IImageResizer FindResizer(IImageFormat imageFormat);
}