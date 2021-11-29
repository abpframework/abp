using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    public interface IBundlerContext
    {
        string BundleRelativePath { get; }

        IReadOnlyList<string> ContentFiles { get; }

        bool IsMinificationEnabled { get; }
    }
}