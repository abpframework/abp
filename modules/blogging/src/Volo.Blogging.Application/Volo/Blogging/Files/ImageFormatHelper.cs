using System;
using System.Collections.Generic;
using System.IO;
using SkiaSharp;

namespace Volo.Blogging.Areas.Blog.Helpers
{
    public class ImageFormatHelper
    {
        public static SKEncodedImageFormat? GetImageRawFormat(Stream stream)
        {
            // Only the image header is parsed, the pixels are not decoded.
            // SKManagedStream rewinds to the absolute start of the stream, so the remaining bytes are copied
            // to give the codec an independent buffer that starts at the current position.
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);

                using (var data = SKData.CreateCopy(memoryStream.GetBuffer(), (ulong)memoryStream.Length))
                using (var codec = SKCodec.Create(data))
                {
                    return codec?.EncodedFormat;
                }
            }
        }

        public static bool IsValidImage(Stream stream, ICollection<SKEncodedImageFormat> validFormats)
        {
            return GetValidImageFormat(stream, validFormats).HasValue;
        }

        public static SKEncodedImageFormat? GetValidImageFormat(Stream stream, ICollection<SKEncodedImageFormat> validFormats)
        {
            try
            {
                var imageFormat = GetImageRawFormat(stream);

                return imageFormat.HasValue && validFormats.Contains(imageFormat.Value) ? imageFormat : null;
            }
            catch (Exception e) when (e is not DllNotFoundException && e is not TypeInitializationException)
            {
                return null;
            }
        }
    }
}
