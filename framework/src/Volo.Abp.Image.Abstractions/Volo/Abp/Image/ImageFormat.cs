namespace Volo.Abp.Image;

public class ImageFormat : IImageFormat
{
    public ImageFormat(string mimeType)
    {
        MimeType = mimeType;
    }

    public string MimeType { get; }
}