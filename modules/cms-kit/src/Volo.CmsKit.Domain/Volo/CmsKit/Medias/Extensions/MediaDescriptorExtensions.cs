using System.IO;
using System.Linq;

namespace Volo.CmsKit.Medias.Extensions
{
    public static class MediaDescriptorExtensions
    {
        public static bool IsValidMediaFileName(this string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }
            
            // "con" is not valid folder/file name for Windows OS
            return !(Path.GetInvalidFileNameChars().Any(name.Contains) || name == "con");
        }
    }
}