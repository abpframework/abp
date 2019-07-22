using System;
using System.Threading.Tasks;

namespace DashboardDemo
{
    public class DashboardAppService : DashboardDemoAppService, IDashboardAppService
    {
        public async Task<CountersWidgetResultDto> GetCountersWidgetAsync(CountersWidgetInputDto input)
        {
            /* Instead of hard-coded / calculated values, get counts from a repository!
           */

            var dayFactor = (int)Math.Round(input.EndDate.Subtract(input.StartDate).TotalDays + 1);
            return new CountersWidgetResultDto
            {
                NewUsers = dayFactor * 86,
                ActiveUsers = dayFactor * 58,
                TotalIncome = dayFactor * 749.53,
                TotalProfit = dayFactor * 239.45,
            };
        }
    }
}
