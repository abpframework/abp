using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    public class BundlerContext : IBundlerContext
    {
        public string BundleRelativePath { get; }

        public IReadOnlyList<string> ContentFiles { get; }

        public bool IsMinificationEnabled { get; }

        public BundlerContext(string bundleRelativePath, IReadOnlyList<string> contentFiles, bool isMinificationEnabled)
        {
            BundleRelativePath = bundleRelativePath;
            ContentFiles = contentFiles;
            IsMinificationEnabled = isMinificationEnabled;
        }
    }
}