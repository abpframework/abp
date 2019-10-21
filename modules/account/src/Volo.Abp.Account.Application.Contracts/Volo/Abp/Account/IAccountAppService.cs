using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;

namespace Volo.Abp.Account
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IdentityUserDto> RegisterAsync(RegisterDto input);
    }
}