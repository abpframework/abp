namespace Volo.Abp.Image;

public interface IImageFormat
{
    string Name { get; }
    string MimeType { get; }
}