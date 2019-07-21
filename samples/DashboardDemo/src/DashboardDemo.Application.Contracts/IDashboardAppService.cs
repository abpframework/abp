using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace DashboardDemo
{
    public interface IDashboardAppService : IApplicationService
    {
        Task<CountersWidgetResultDto> GetCountersWidgetAsync(CountersWidgetInputDto input);
    }

    public class CountersWidgetInputDto
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }

    public class CountersWidgetResultDto
    {
        public int NewUsers { get; set; }
        public int ActiveUsers { get; set; }
        public double TotalIncome { get; set; }
        public double TotalProfit { get; set; }
    }
}
