using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Dashboards
{
    public interface IDashboardRenderer : ITransientDependency
    {
        Task<IHtmlContent> RenderAsync(IViewComponentHelper componentHelper, object args = null);
    }
}
