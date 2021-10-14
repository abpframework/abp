using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers
{
    public class AbpTagHelperScriptStyleLoadingOptions
    {
        public bool GlobalDeferScript { get; set; }

        public List<string> DeferScripts { get; }

        public bool GlobalPreloadStyle { get; set; }

        public List<string> PreloadStyles { get; }

        public AbpTagHelperScriptStyleLoadingOptions()
        {
            GlobalDeferScript = false;
            DeferScripts = new List<string>();

            GlobalPreloadStyle = false;
            PreloadStyles = new List<string>();
        }
    }
}
