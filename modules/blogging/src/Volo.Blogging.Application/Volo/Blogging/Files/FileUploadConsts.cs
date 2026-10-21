using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SkiaSharp;

namespace Volo.Blogging.Files
{
    public class FileUploadConsts
    {
        public static readonly ICollection<SKEncodedImageFormat> AllowedImageUploadFormats = new Collection<SKEncodedImageFormat>
        {
            SKEncodedImageFormat.Jpeg,
            SKEncodedImageFormat.Png,
            SKEncodedImageFormat.Gif,
            SKEncodedImageFormat.Bmp,
        };

        public static string AllowedImageFormatsJoint => string.Join(",", AllowedImageUploadFormats.Select(x => x.ToString().ToUpperInvariant()));
    }
}
