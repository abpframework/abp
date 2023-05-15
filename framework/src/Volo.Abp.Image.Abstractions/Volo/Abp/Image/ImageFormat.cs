namespace Volo.Abp.Image;

public class ImageFormat : IImageFormat
{
    public ImageFormat(string name, string mimeType)
    {
        Name = name;
        MimeType = mimeType;
    }

    public string Name { get; }
    public string MimeType { get; }
}