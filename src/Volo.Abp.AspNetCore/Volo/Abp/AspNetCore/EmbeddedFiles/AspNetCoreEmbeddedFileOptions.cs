using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.EmbeddedFiles
{
    internal class AspNetCoreEmbeddedFileOptions
    {
        public HashSet<string> IgnoredFileExtensions { get; }

        public AspNetCoreEmbeddedFileOptions()
        {
            IgnoredFileExtensions = new HashSet<string>
            {
                "cshtml",
                "config"
            };
        }
    }
}