namespace Volo.Abp.Image;

public enum ImageResizeMode
{
    None,
    Stretch,
    BoxPad,
    Min,
    Max,
    Crop,
    Pad,
    Fill,
    Distort,
    Default = Stretch
}