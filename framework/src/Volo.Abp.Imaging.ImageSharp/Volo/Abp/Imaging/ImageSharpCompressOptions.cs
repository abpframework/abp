using SixLabors.ImageSharp.Formats.Png;

namespace Volo.Abp.Imaging;

public class ImageSharpCompressOptions
{
    public int JpegQuality { get; set; } = 60;
    public PngCompressionLevel PngCompressionLevel { get; set; } = PngCompressionLevel.BestCompression;
    public bool PngIgnoreMetadata { get; set; } = true;
    public int WebpQuality { get; set; } = 60;
}