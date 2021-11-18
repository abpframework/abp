using System;
using System.Collections.Generic;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;

namespace Volo.Blogging.Areas.Blog.Helpers
{
    public class ImageFormatHelper
    {
        public static IImageFormat GetImageRawFormat(Stream stream)
        {
            using (var image = Image.Load(stream, out var imageFormat))
            {
                return imageFormat;
            }
        }

        public static bool IsValidImage(Stream stream, ICollection<IImageFormat> validFormats)
        {
            try
            {
                var imageFormat = GetImageRawFormat(stream);
                
                return validFormats.Contains(imageFormat);
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
