using System.Reflection;
using System.Threading.Tasks;

namespace Volo.Abp.TextTemplating.Razor
{
    public interface ICompiledViewProvider
    {
        Task<Assembly> GetAssemblyAsync(TemplateDefinition templateDefinition);
    }
}
