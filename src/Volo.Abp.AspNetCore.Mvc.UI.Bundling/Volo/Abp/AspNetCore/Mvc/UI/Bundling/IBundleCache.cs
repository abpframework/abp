using System;
using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    public interface IBundleCache
    {
        List<string> GetFiles(string bundleName, Func<List<string>> factory);
    }
}