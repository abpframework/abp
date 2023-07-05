using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Blogging.Posts;

namespace Volo.Blogging.Members;

public interface IMemberAppService : IApplicationService
{
    Task<BlogUserDto> FindAsync(string username);
    
    Task UpdateUserProfileAsync(CustomIdentityBlogUserUpdateDto input);
}