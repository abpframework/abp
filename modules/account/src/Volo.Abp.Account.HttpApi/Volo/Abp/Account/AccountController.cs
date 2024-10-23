using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;

namespace Volo.Abp.Account;

[RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
[Area(AccountRemoteServiceConsts.ModuleName)]
[Route("api/account")]
public class AccountController(IAccountAppService accountAppService) : AbpControllerBase, IAccountAppService
{
    protected IAccountAppService AccountAppService { get; } = accountAppService;

    [HttpPost]
    [Route("register")]
    public virtual Task<IdentityUserDto> RegisterAsync(RegisterDto input) => 
        AccountAppService.RegisterAsync(input);

    [HttpPost]
    [Route("send-password-reset-code")]
    public virtual Task SendPasswordResetCodeAsync(SendPasswordResetCodeDto input) => 
        AccountAppService.SendPasswordResetCodeAsync(input);

    [HttpPost]
    [Route("verify-password-reset-token")]
    public virtual Task<bool> VerifyPasswordResetTokenAsync(VerifyPasswordResetTokenInput input) => 
        AccountAppService.VerifyPasswordResetTokenAsync(input);

    [HttpPost]
    [Route("reset-password")]
    public virtual Task ResetPasswordAsync(ResetPasswordDto input) => 
        AccountAppService.ResetPasswordAsync(input);
}
