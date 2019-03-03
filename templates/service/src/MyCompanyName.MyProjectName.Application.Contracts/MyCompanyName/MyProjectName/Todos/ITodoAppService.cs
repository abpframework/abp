using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace MyCompanyName.MyProjectName.Todos
{
    public interface ITodoAppService
    {
        Task<PagedResultDto<TodoDto>> GetListAsync();
    }
}