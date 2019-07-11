using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace DashboardDemo
{
    public interface IDemoStatisticAppService : IApplicationService
    {
        Task<NewUserPerDayStatisticDto> GetNewUserPerDayStatistic();

        Task<MonthlyProfitStatisticDto> GetMonthlyProfitStatistic();
    }
}
