using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.VirtualFileSystem
{
    public class AspNetCoreContentOptions
    {
        public List<string> AllowedExtraWebContentFolders { get; }
        public List<string> AllowedExtraWebContentFileExtensions { get; }

        public AspNetCoreContentOptions()
        {
            AllowedExtraWebContentFolders = new List<string>
            {
                "/Pages",
                "/Views",
                "/Themes"
            };

            AllowedExtraWebContentFileExtensions = new List<string>
            {
                ".js",
                ".css",
                ".png",
                ".jpg",
                ".jpeg",
                ".woff",
                ".woff2",
                ".tff",
                ".otf"
            };
        }
    }
}
