using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace DashboardDemo
{
    public interface IDashboardAppService : IApplicationService
    {
        Task<CountersWidgetResultDto> GetCountersWidgetAsync(CountersWidgetInputDto input);
    }
}
