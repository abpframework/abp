using SkiaSharp;

namespace Volo.Abp.Imaging;

public class SkiaSharpResizerOptions
{
    public SKFilterQuality SKFilterQuality { get; set; }

    public int Quality { get; set; }

    public SkiaSharpResizerOptions()
    {
        SKFilterQuality = SKFilterQuality.None;
        Quality = 75;
    }
}
