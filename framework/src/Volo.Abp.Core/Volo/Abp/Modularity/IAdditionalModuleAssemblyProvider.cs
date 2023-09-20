using System.Reflection;

namespace Volo.Abp.Modularity;

public interface IAdditionalModuleAssemblyProvider
{
    Assembly[] GetAssemblies();
}