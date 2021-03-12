using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Authorization;

namespace Volo.CmsKit.Admin
{
    public abstract class CmsKitAdminAppServiceBase : CmsKitAppServiceBase
    {
        protected CmsKitAdminAppServiceBase()
        {
            ObjectMapperContext = typeof(CmsKitAdminApplicationModule);
        }

        /// <summary>
        /// Checks given policies until finding granted policy. If none of them is granted, throws <see cref="AbpAuthorizationException"/>
        /// </summary>
        /// <param name="policies">Policies to be checked.</param>
        /// <exception cref="AbpAuthorizationException">Thrown when none of policies is granted.</exception>
        protected async Task CheckAnyOfPoliciesAsync([NotNull] IEnumerable<string> policies)
        {
            Check.NotNull(policies, nameof(policies));

            foreach (var policy in policies)
            {
                if (await AuthorizationService.IsGrantedAsync(policy))
                {
                    return;
                }
            }

            throw new AbpAuthorizationException();
        }
    }
}
