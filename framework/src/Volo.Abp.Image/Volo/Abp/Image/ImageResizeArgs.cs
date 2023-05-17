namespace Volo.Abp.Image;

public class ImageResizeArgs : IImageResizeParameter
{
    public int? Width { get; set; }
    
    public int? Height { get; set; }
    
    public ImageResizeMode? Mode { get; set; } //TODO: Set default here

    public ImageResizeArgs(int? width = null, int? height = null, ImageResizeMode? mode = null)
    {
        Mode = mode;
        Width = width;
        Height = height;
        
        //TODO: Throw exception if width/height is not null and negative
    }
}