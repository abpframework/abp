using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using OpenIddict.Abstractions;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.OpenIddict;

public class AbpOpenIddictRequestHelper : ITransientDependency
{
    public virtual Task<OpenIddictRequest> GetFromReturnUrlAsync(string returnUrl)
    {
        if (!returnUrl.IsNullOrWhiteSpace())
        {
            var qm = returnUrl.IndexOf("?", StringComparison.Ordinal);
            if (qm > 0)
            {
                return Task.FromResult(new OpenIddictRequest(returnUrl.Substring(qm + 1)
                    .Split("&")
                    .Select(x =>
                        x.Split("=").Length == 2
                            ? new KeyValuePair<string, string>(x.Split("=")[0], WebUtility.UrlDecode(x.Split("=")[1]))
                            : new KeyValuePair<string, string>(null, null))
                    .Where(x => x.Key != null)));
            }
        }

        return Task.FromResult<OpenIddictRequest>(null);
    }
}
