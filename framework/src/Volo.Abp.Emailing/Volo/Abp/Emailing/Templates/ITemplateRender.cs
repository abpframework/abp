using System.Threading.Tasks;

namespace Volo.Abp.Emailing.Templates
{
    public interface ITemplateRender
    {
        Task<string> RenderAsync(string template, object model = null);
    }
}