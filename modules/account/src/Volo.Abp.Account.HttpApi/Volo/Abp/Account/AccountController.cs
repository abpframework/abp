using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;

namespace Volo.Abp.Account
{
    [RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
    [Area("account")]
    [Route("api/account")]
    public class AccountController : AbpController, IAccountAppService
    {
        protected IAccountAppService AccountAppService { get; }

        public AccountController(IAccountAppService accountAppService)
        {
            AccountAppService = accountAppService;
        }

        [HttpPost]
        [Route("register")]
        public virtual Task<IdentityUserDto> RegisterAsync(RegisterDto input)
        {
            return AccountAppService.RegisterAsync(input);
        }
    }
}