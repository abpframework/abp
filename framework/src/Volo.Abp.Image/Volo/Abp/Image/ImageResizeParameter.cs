namespace Volo.Abp.Image;

public class ImageResizeParameter : IImageResizeParameter
{
    public ImageResizeParameter(int? width = null, int? height = null, ImageResizeMode? mode = null)
    {
        Mode = mode;
        Width = width;
        Height = height;
    }

    public int? Width { get; set; }
    public int? Height { get; set; }
    public ImageResizeMode? Mode { get; set; }
}