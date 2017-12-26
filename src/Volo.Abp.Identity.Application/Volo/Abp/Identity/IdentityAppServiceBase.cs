using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.Application.Services;

namespace Volo.Abp.Identity
{
    public abstract class IdentityAppServiceBase : ApplicationService
    {
        protected void CheckIdentityErrors(IdentityResult identityResult)
        {
            if (!identityResult.Succeeded)
            {
                //TODO: A better exception that can be shown on UI as localized?
                throw new AbpException("Operation failed: " + identityResult.Errors.Select(e => $"[{e.Code}] {e.Description}").JoinAsString(", "));
            }

            //identityResult.CheckErrors(LocalizationManager); //TODO: Get from old Abp
        }
    }
}
