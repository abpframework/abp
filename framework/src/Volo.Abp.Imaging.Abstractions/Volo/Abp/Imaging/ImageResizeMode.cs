namespace Volo.Abp.Imaging;

public enum ImageResizeMode : byte
{
    None = 0,
    Stretch = 1,
    BoxPad = 2,
    Min = 3,
    Max = 4,
    Crop = 5,
    Pad = 6,
    Default = 7
}