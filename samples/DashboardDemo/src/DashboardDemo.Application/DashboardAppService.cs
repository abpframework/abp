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

        public async Task<NewUserStatisticWidgetResultDto> GetNewUserStatisticWidgetAsync(NewUserStatisticWidgetInputDto input)
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

        public async Task<LicenseStatistiWidgetResultDto> GetLicenseStatisticWidgetAsync(LicenseStatisticWidgetInputDto input)
        {
            var days = (int)Math.Round(input.EndDate.Subtract(input.StartDate).TotalDays + 1);

            var data = new Dictionary<string, int>
            {
                {"Regular", new Random().Next(50, 100) * days},
                {"Enterprise", new Random().Next(40, 70) * days},
                {"Ultimate", new Random().Next(30, 40) * days}
            };

            return new LicenseStatistiWidgetResultDto() { Data = data };
        }

        private async Task<NewUserStatisticWidgetResultDto> GetMonthlyNewUserStatisticWidgetAsync(NewUserStatisticWidgetInputDto input)
        {
            DateTime endDate = input.EndDate;
            DateTime startDate = input.StartDate;

            var months = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sept", "Oct", "Nov", "Dec" };
            var monthCount = (endDate.Year - startDate.Year) * 12 + endDate.Month - startDate.Month + 1;
            var labels = new List<string>();

            for (var i = 0; i < monthCount; i++)
            {
                labels.Add(months[endDate.Month - 1] + " " + endDate.Year);
                endDate = endDate.AddMonths(-1);
            }

            labels.Reverse();

            var data = new Dictionary<string, int>();

            for (var i = 0; i < monthCount; i++)
            {
                data.Add(labels[i], new Random().Next(1500, 3000));
            }

            return new NewUserStatisticWidgetResultDto()
            {
                Data = data
            };
        }

        private async Task<NewUserStatisticWidgetResultDto> GetDailyNewUserStatisticWidgetAsync(NewUserStatisticWidgetInputDto input)
        {
            var dayCount = (input.EndDate - input.StartDate).Days + 1;

            var labels = new List<string>();

            for (int i = 0; i < dayCount; i++)
            {
                labels.Add(input.StartDate.AddDays(i).ToString("dd/MM/yyyy"));
            }

            var data = new Dictionary<string, int>();

            for (var i = 0; i < dayCount; i++)
            {
                data.Add(labels[i], new Random().Next(50, 100));
            }

            return new NewUserStatisticWidgetResultDto()
            {
                Data = data
            };
        }
    }
}