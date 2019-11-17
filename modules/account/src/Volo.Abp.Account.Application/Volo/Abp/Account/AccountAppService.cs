using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;

namespace Volo.Abp.Account
{
    public class AccountAppService : ApplicationService, IAccountAppService
    {
        protected IdentityUserManager UserManager { get; }

        public AccountAppService(
            IdentityUserManager userManager)
        {
            UserManager = userManager;
        }

        public async Task<IdentityUserDto> RegisterAsync(RegisterDto input)
        {
            var user = new IdentityUser(GuidGenerator.Create(), input.UserName, input.EmailAddress, CurrentTenant.Id);

            (await UserManager.CreateAsync(user, input.Password)).CheckErrors();

            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }
    }
}
