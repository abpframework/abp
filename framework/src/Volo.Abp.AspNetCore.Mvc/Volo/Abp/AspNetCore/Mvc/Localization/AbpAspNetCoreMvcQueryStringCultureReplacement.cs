using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Localization;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.Localization
{
    public class AbpAspNetCoreMvcQueryStringCultureReplacement : IQueryStringCultureReplacement, ITransientDependency
    {
        public virtual Task<string> ReplaceAsync(string returnUrl, RequestCulture requestCulture)
        {
            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                if (returnUrl.Contains("culture=", StringComparison.OrdinalIgnoreCase))
                {
                    returnUrl = Regex.Replace(returnUrl, "culture=[A-Za-z-]+?&", $"culture={requestCulture.Culture}&", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                }
                if (returnUrl.Contains("ui-Culture=", StringComparison.OrdinalIgnoreCase))
                {
                    returnUrl = Regex.Replace(returnUrl, "ui-culture=[A-Za-z-]+?$", $"ui-culture={requestCulture.UICulture}",RegexOptions.Compiled | RegexOptions.IgnoreCase);
                }
            }

            return Task.FromResult<string>(returnUrl);
        }
    }
}
