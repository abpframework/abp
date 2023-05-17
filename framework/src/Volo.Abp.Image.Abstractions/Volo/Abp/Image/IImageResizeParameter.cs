namespace Volo.Abp.Image;

public interface IImageResizeParameter //TODO: Remove
{
    int? Width { get; }
    int? Height { get; }

    ImageResizeMode? Mode { get; }
}