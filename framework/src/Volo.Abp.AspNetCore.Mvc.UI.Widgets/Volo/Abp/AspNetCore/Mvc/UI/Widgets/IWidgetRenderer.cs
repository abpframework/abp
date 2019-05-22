using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Widgets
{
    public interface IWidgetRenderer : ITransientDependency
    {
        Task<IHtmlContent> RenderAsync(IViewComponentHelper componentHelper, string widgetName, object args = null);
    }
}
