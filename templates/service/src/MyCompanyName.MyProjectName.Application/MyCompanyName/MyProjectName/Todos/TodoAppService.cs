using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MyCompanyName.MyProjectName.Todos
{
    public class TodoAppService : ApplicationService, ITodoAppService
    {
        public Task<PagedResultDto<TodoDto>> GetListAsync()
        {
            return Task.FromResult(
                new PagedResultDto<TodoDto>(2, new List<TodoDto>
                    {
                        new TodoDto {Id = GuidGenerator.Create(), Text = "Todo item one"},
                        new TodoDto {Id = GuidGenerator.Create(), Text = "Todo item two"}
                    }
                ));
        }
    }
}
