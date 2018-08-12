using System;
using Volo.Abp.Account.Web.Areas.Account.Controllers.Models;
using Volo.Abp.Identity;

namespace Volo.Abp.Account.Web.Areas.Account.Controllers
{
    public class AbpLoginResult
    {
        public AbpLoginResult(LoginResultType result)
        {
            Result = result;
        }

        public string IdentityCookieToken { get; set; }

        public LoginResultType Result { get; }

        public string Description => Result.ToString();
    }
}