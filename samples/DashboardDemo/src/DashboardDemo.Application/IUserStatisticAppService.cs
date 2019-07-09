using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace DashboardDemo
{
    public interface IUserStatisticAppService : IApplicationService
    {
        Task<NewUserPerDayStatisticDto> GetNewUserPerDayStatistic();
    }
}
