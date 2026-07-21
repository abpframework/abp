namespace Volo.Abp.Imaging;

public class SkiaSharpCompressOptions
{
    public int Quality { get; set; }

    public SkiaSharpCompressOptions()
    {
        Quality = 75;
    }
}
