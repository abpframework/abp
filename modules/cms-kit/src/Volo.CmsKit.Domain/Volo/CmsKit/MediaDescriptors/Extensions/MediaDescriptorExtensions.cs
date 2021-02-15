using System.IO;
using System.Linq;

namespace Volo.CmsKit.MediaDescriptors.Extensions
{
    public static class MediaDescriptorExtensions
    {
        public static bool IsValidMediaFileName(this string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }
            
            return !Path.GetInvalidFileNameChars().Any(name.Contains);
        }
    }
}