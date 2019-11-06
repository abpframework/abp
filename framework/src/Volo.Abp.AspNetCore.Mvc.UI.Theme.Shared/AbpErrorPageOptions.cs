using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared
{
    public class AbpErrorPageOptions
    {
        public readonly IDictionary<string, string> ErrorPageUrls;

        public AbpErrorPageOptions()
        {
            ErrorPageUrls = new Dictionary<string, string>();
        }
    }
}