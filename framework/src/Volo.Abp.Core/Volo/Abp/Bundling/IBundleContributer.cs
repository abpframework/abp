using System;
using System.Collections.Generic;
using System.Text;

namespace Volo.Abp.Bundling
{
    public interface IBundleContributer
    {
        void AddScripts(List<string> scripts);
        void AddStyles(List<string> styles);
    }
}
