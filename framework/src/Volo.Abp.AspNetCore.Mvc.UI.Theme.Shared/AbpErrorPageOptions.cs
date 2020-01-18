using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared
{
    public class AbpErrorPageOptions
    {
        public readonly IDictionary<string, string> ErrorViewUrls;

        public AbpErrorPageOptions()
        {
            ErrorViewUrls = new Dictionary<string, string>();
        }
    }
}