using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace DashboardDemo
{
    public interface IDashboardAppService : IApplicationService
    {
        Task<CountersWidgetResultDto> GetCountersWidgetAsync(CountersWidgetInputDto input);

        Task<NewUserStatistiWidgetResultDto> GetNewUserStatisticWidgetAsync(NewUserStatisticWidgetInputDto input);

        Task<LicenseStatistiWidgetResultDto> GetLicenseStatisticWidgetAsync(LicenseStatisticWidgetInputDto input);
    }
}
