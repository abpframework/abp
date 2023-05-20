using System;

namespace Volo.Abp.Image;

public class ImageResizeArgs
{
    public int Width { get; set; }
    
    public int Height { get; set; }
    
    public ImageResizeMode Mode { get; set; } = ImageResizeMode.Default;

    public ImageResizeArgs(int? width = null, int? height = null, ImageResizeMode? mode = null)
    {
        if (mode.HasValue)
        {
            Mode = mode.Value;
        }
        
        if (width is < 0)
        {
            throw new ArgumentException("Width cannot be negative!", nameof(width));
        }
        
        if (height is < 0)
        {
            throw new ArgumentException("Height cannot be negative!", nameof(height));
        }
        
        Width = width ?? 0;
        Height = height ?? 0;
    }
}