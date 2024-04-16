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
    
    public virtual async Task<BlogUserDto> FindAsync(string username)
    {
        var user = await _userRepository.FindAsync(x => x.UserName == username && x.IsActive);
        
        if (user == null)
        {
            return null;
        }
        
        return ObjectMapper.Map<BlogUser, BlogUserDto>(user);
    }

    public virtual async Task UpdateUserProfileAsync(CustomIdentityBlogUserUpdateDto input)
    {
        var user = await _userRepository.GetAsync(CurrentUser.Id.Value);

        user.Name = input.Name;
        user.Surname = input.Surname;
        user.WebSite = input.WebSite;
        user.Twitter = input.Twitter;
        user.Github = input.Github;
        user.Linkedin = input.Linkedin;
        user.Company = input.Company;
        user.JobTitle = input.JobTitle;
        user.Biography = input.Biography;
            
        await _userRepository.UpdateAsync(user);
    }
}