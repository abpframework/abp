using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.CmsKit.Web.Contents;

public class PlainTextContentRenderer : IContentRenderer, ITransientDependency
{
    public Task<string> RenderAsync(string value)
    {
        return Task.FromResult(value);
    }
}
