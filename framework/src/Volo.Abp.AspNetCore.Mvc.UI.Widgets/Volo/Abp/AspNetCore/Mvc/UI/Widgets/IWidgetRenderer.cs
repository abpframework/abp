using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Widgets
{
    public interface IWidgetRenderer : ITransientDependency
    {
        Task<object> RenderAsync(string mywidget);
    }

    public class WidgetRenderer : IWidgetRenderer
    {
        public Task<object> RenderAsync(string mywidget)
        {
            throw new System.NotImplementedException();
        }
    }
}
