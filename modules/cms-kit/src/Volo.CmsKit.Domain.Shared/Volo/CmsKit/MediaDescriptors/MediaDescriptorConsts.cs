using Volo.CmsKit.Entities;

namespace Volo.CmsKit.MediaDescriptors;

public class MediaDescriptorConsts
{
    public static int MaxEntityTypeLength { get; set; } = CmsEntityConsts.MaxEntityTypeLength;

    public static int MaxNameLength { get; set; } = 255;

    public static int MaxMimeTypeLength { get; set; } = 128;

    public static int MaxSizeLength = int.MaxValue;
}
