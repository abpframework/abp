using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.TextTemplating
{
    public interface ITemplateRenderer
    {
        Task<string> RenderAsync(
            [NotNull] string templateName,
            [CanBeNull] string cultureName = null
        );
    }
}