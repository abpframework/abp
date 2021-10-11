using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace Volo.Blogging.Areas.Blog.Helpers
{
    public class ImageFormatHelper
    {
        public static ImageFormat GetImageRawFormat(Stream stream)
        {
            return System.Drawing.Image.FromStream(stream).RawFormat;
        }

        public static bool IsValidImage(Stream stream, ICollection<ImageFormat> validFormats)
        {
            // System.Drawing only works on windows => https://docs.microsoft.com/en-us/dotnet/api/system.drawing.image?view=net-5.0#remarks
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var imageFormat = GetImageRawFormat(stream);
                return validFormats.Contains(imageFormat);
            }

            return true;
        }
    }
}
