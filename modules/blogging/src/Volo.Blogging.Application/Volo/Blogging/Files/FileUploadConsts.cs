using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;

namespace Volo.Blogging.Files
{
    public class FileUploadConsts
    {
        public static readonly ICollection<IImageFormat> AllowedImageUploadFormats = new Collection<IImageFormat>
        {
            JpegFormat.Instance, 
            PngFormat.Instance, 
            GifFormat.Instance, 
            BmpFormat.Instance,
        };

        public static string AllowedImageFormatsJoint => string.Join(",", AllowedImageUploadFormats.Select(x => x.Name));
    }
}
