using System;
using System.Collections.Generic;
using System.Text;
using DashboardDemo.Localization;
using Volo.Abp.Application.Services;

namespace DashboardDemo
{
    /* Inherit your application services from this class.
     */
    public abstract class DashboardDemoAppService : ApplicationService
    {
        protected DashboardDemoAppService()
        {
            LocalizationResource = typeof(DashboardDemoResource);
        }
    }
}
