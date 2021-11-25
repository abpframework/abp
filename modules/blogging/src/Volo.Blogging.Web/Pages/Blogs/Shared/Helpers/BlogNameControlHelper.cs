using System.IO;
using System.Linq;

namespace Volo.Blogging.Pages.Blogs.Shared.Helpers
{
    public static class BlogNameControlHelper
    {
        public static readonly string[] ProhibitedFileExtensions = new string[] {"ico", "txt", "php"};

        public static bool IsProhibitedFileFormatName(string blogShortName)
        {
            if (string.IsNullOrWhiteSpace(blogShortName))
            {
                return false;
            }

            return ProhibitedFileExtensions.Any(x => blogShortName.ToLowerInvariant().EndsWith(x));
        }
    }
}