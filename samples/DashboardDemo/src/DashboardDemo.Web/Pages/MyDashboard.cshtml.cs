using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DashboardDemo.Web.Pages
{
    public class MyDashboardModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public DateTime? StartDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? EndDate { get; set; }

        public void OnGet()
        {
            if (StartDate == null)
            {
                StartDate = DateTime.Now.Subtract(TimeSpan.FromDays(6)).ClearTime();
            }

            if (EndDate == null)
            {
                EndDate = DateTime.Now.AddDays(1).ClearTime();
            }
        }
    }
}