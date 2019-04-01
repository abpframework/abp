using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.Identity
{
    [RemoteService]
    [Area("identity")]
    [ControllerName("Profile")]
    public class ProfileController : AbpController, IProfileAppService
    {
        private readonly IProfileAppService _profileAppService;

        public ProfileController(IProfileAppService profileAppService)
        {
            _profileAppService = profileAppService;
        }
        public Task<ProfileDto> GetAsync()
        {
            return _profileAppService.GetAsync();
        }

        public Task<ProfileDto> UpdateAsync(UpdateProfileDto input)
        {
            return _profileAppService.UpdateAsync(input);
        }

        public Task ChangePasswordAsync(string currentPassword, string newPassword)
        {
            return _profileAppService.ChangePasswordAsync(currentPassword, newPassword);
        }
    }
}
