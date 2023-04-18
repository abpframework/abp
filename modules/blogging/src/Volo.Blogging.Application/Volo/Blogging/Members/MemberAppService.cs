using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Blogging.Posts;
using Volo.Blogging.Users;

namespace Volo.Blogging.Members;

public class MemberAppService : BloggingAppServiceBase, IMemberAppService
{
    private readonly IRepository<BlogUser, Guid> _userRepository;

    public MemberAppService(IRepository<BlogUser, Guid> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<BlogUserDto> FindAsync(string username)
    {
        var user = await _userRepository.FindAsync(x => x.UserName == username);
        
        return ObjectMapper.Map<BlogUser, BlogUserDto>(user);
    }
}