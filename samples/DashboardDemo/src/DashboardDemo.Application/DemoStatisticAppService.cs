using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace DashboardDemo
{
    public class DemoStatisticAppService : ApplicationService, IDemoStatisticAppService
    {
        public async Task<GetMonthlyUserStatisticDto> GetMonthlyUserStatistic(FilterDto filter)
        {
            var monthCount = GetLabels(filter, out var monthList);

            var data = Enumerable
                .Repeat(0, monthCount)
                .Select(i => new Random().Next(750, 960))
                .ToArray();

            return new GetMonthlyUserStatisticDto{Labels = monthList.ToArray(), Data = data};
        }

        public async Task<MonthlyProfitStatisticDto> GetMonthlyProfitStatistic(FilterDto filter)
        {
            var monthCount = GetLabels(filter, out var monthList);

            var data = Enumerable
                .Repeat(0, monthCount)
                .Select(i => new Random().Next(-20, 40))
                .ToArray();

            return new MonthlyProfitStatisticDto { Labels = monthList.ToArray(), Data = data };
        }

        private static int GetLabels(FilterDto filter, out List<string> monthList)
        {
            DateTime endDate = filter.EndDate ?? DateTime.Now;
            DateTime startDate = filter.StartDate ?? DateTime.Now.AddYears(-1);

            if (filter.StartDate > filter.EndDate)
            {
                throw new BusinessException("Start date can not be greater than end date.");
            }

            var months = new[] {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sept", "Oct", "Nov", "Dec"};
            var monthCount = (endDate.Year - startDate.Year) * 12 + endDate.Month - startDate.Month +1;
            monthList = new List<string>();

            for (int i = 0; i < monthCount; i++)
            {
                monthList.Add(months[endDate.Month-1]);
                endDate = endDate.AddMonths(-1);
            }


            monthList.Reverse();

            return monthCount;
        }
    }
}
