namespace Volo.Abp.Image;

public interface IImageResizeParameter
{
    int? Width { get; }
    int? Height { get; }

    ImageResizeMode? Mode { get; }
}