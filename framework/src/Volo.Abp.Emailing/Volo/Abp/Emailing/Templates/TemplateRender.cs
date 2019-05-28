using System.Threading.Tasks;
using Scriban;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Emailing.Templates
{
    public class TemplateRender : ITemplateRender, ITransientDependency
    {
        public async Task<string> RenderAsync(string template, object model = null)
        {
            var scribanTemplate = Template.Parse(template);
            return await scribanTemplate.RenderAsync(model);
        }
    }
}