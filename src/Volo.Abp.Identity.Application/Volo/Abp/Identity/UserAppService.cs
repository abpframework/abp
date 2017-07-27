using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Application.Services.Dtos;

namespace Volo.Abp.Identity
{
    //TODO: Consider a way of passing cancellation token to all async application service methods!

    public class UserAppService : ApplicationService, IUserAppService
    {
        private readonly IIdentityUserRepository _userRepository;

        public UserAppService(IIdentityUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ListResultDto<IdentityUserDto>> GetAll()
        {
            var users = await _userRepository.GetListAsync();

            return new ListResultDto<IdentityUserDto>(
                ObjectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>(users)
            );
        }

        public async Task<IdentityUserDto> Get(Guid id)
        {
            var user = await _userRepository.GetAsync(id);

            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }
    }
}
