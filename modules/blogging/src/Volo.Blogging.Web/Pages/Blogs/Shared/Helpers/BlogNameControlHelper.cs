using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace Volo.Blogging.Pages.Blogs.Shared.Helpers
{
    public static class BlogNameControlHelper
    {
        public static bool IsFileFormat(string blogShortName)
        {
            if (!string.IsNullOrWhiteSpace(blogShortName))
            {
                var fileInfo = new FileInfo(blogShortName);

                if (!string.IsNullOrEmpty(fileInfo.Extension))
                {
                    return true;
                }
            }

            return false;
        }
    }
}