using System.Threading.Tasks;

namespace Volo.Abp.Studio.Packages.Modifying;

public interface IAbpModuleFileManager
{
    Task AddDependency(string filePath, string moduleToAdd);

    Task<string> ExtractModuleNameWithNamespace(string filePath);
}
