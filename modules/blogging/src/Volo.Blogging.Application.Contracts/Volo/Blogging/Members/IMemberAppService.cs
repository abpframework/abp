using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Blogging.Posts;

namespace Volo.Blogging.Members;

public interface IMemberAppService : IApplicationService
{
    Task<BlogUserDto> GetAsync(string username);
}