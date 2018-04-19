using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    public interface IBundler
    {
        string CreateBundle(List<string> files);
    }
}