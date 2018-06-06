using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    public interface IBundleContributor
    {
        void Contribute(List<string> files);
    }
}
