using System.Reflection;
using System.Threading.Tasks;

namespace Volo.Abp.TextTemplating.Razor;

public interface IAbpCompiledViewProvider
{
    Task<Assembly> GetAssemblyAsync(TemplateDefinition templateDefinition);
}
