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
            // SKCodec.Create(Stream) takes the ownership of the stream and disposes it, so the remaining bytes
            // are copied to give the codec its own buffer and keep the stream open for the caller.
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
            catch (Exception e) when (!IsNativeLibraryException(e))
            {
                return null;
            }
        }

        private static bool IsNativeLibraryException(Exception e)
        {
            return e is DllNotFoundException
                or EntryPointNotFoundException
                or BadImageFormatException
                or TypeInitializationException;
        }
    }
}
