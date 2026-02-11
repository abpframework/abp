using SkiaSharp;

namespace Volo.Abp.Imaging;

public class SkiaSharpResizerOptions
{
    public SKSamplingOptions SKSamplingOptions { get; set; }

    public int Quality { get; set; }

    public SkiaSharpResizerOptions()
    {
        SKSamplingOptions = SKSamplingOptions.Default;
        Quality = 75;
    }
}
