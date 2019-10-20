using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.VirtualFileSystem
{
    public class AbpAspNetCoreContentOptions
    {
        public List<string> AllowedExtraWebContentFolders { get; }
        public List<string> AllowedExtraWebContentFileExtensions { get; }

        public AbpAspNetCoreContentOptions()
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
