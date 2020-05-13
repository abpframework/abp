using System.IO;
using System.Linq;

namespace Volo.Blogging.Pages.Blogs.Shared.Helpers
{
    public static class BlogNameControlHelper
    {
        public static readonly string[] ProhibitedFileExtensions = new string[] {".ico", ".txt", ".php"};

        public static bool IsProhibitedFileFormatName(string blogShortName)
        {
            if (!string.IsNullOrWhiteSpace(blogShortName))
            {
                var fileInfo = new FileInfo(blogShortName);

                return ProhibitedFileExtensions.Contains(fileInfo.Extension);
            }

            return false;
        }
    }
}