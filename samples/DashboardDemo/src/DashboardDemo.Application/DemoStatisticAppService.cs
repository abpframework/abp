using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace DashboardDemo
{
    public class DemoStatisticAppService : ApplicationService, IDemoStatisticAppService
    {
        public async Task<NewUserPerDayStatisticDto> GetNewUserPerDayStatistic()
        {
            var data = Enumerable
                .Repeat(0, 7)
                .Select(i => new Random().Next(50, 99))
                .ToArray();

            return new NewUserPerDayStatisticDto{Data = data};
        }

        public async Task<MonthlyProfitStatisticDto> GetMonthlyProfitStatistic()
        {
            var data = Enumerable
                .Repeat(0, 12)
                .Select(i => new Random().Next(-20, 40))
                .ToArray();

            return new MonthlyProfitStatisticDto { Data = data };
        }
    }
}
