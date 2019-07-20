using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Dashboards
{
    public interface IGlobalFilterRenderer : ITransientDependency
    {
        Task<IHtmlContent> RenderAsync(IViewComponentHelper componentHelper, string globalFilterName, object args = null);
    }
}
