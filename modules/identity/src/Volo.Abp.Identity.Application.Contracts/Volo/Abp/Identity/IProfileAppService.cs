using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Abp.Identity
{
    public interface IProfileAppService : IApplicationService
    {
        Task<ProfileDto> GetAsync();

        Task<ProfileDto> UpdateAsync(UpdateProfileDto input);

        Task ChangePasswordAsync(ChangePasswordInput input);
    }
}
