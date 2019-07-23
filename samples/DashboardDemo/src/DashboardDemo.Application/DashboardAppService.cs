using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;

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

        public async Task<NewUserStatistiWidgetResultDto> GetNewUserStatisticWidgetAsync(NewUserStatisticWidgetInputDto input)
        {
            switch (input.Frequency)
            {
                case NewUserStatisticFrequency.Daily:
                    return await GetDailyNewUserStatisticWidgetAsync(input);
                case NewUserStatisticFrequency.Monthly:
                    return await GetMonthlyNewUserStatisticWidgetAsync(input);
            }

            throw new UserFriendlyException("Not implemented statistic frequency.");
        }

        private async Task<NewUserStatistiWidgetResultDto> GetMonthlyNewUserStatisticWidgetAsync(NewUserStatisticWidgetInputDto input)
        {
            DateTime endDate = input.EndDate;
            DateTime startDate = input.StartDate;

            var months = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sept", "Oct", "Nov", "Dec" };
            var monthCount = (endDate.Year - startDate.Year) * 12 + endDate.Month - startDate.Month + 1;
            var labels = new List<string>();

            for (int i = 0; i < monthCount; i++)
            {
                labels.Add(months[endDate.Month - 1]);
                endDate = endDate.AddMonths(-1);
            }

            labels.Reverse();

            var data = Enumerable
                .Repeat(0, monthCount)
                .Select(i => new Random().Next(1500, 3000))
                .ToArray();


            return new NewUserStatistiWidgetResultDto()
            {
                Data = data,
                Labels = labels.ToArray()
            };
        }

        private async Task<NewUserStatistiWidgetResultDto> GetDailyNewUserStatisticWidgetAsync(NewUserStatisticWidgetInputDto input)
        {
            var dayCount = (input.EndDate - input.StartDate).Days + 1;

            var data = Enumerable
                .Repeat(0, dayCount)
                .Select(i => new Random().Next(50, 100))
                .ToArray();

            var labels = new List<string>();

            for (int i = 0; i < dayCount; i++)
            {
                labels.Add(input.StartDate.AddDays(i).Day.ToString());
            }

            return new NewUserStatistiWidgetResultDto()
            {
                Data = data,
                Labels = labels.ToArray()
            };
        }
    }
}
