using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Studio.Package;

namespace Volo.Abp.Studio.Packages.Modifying;

public interface ICsprojFileManager
{
    Task AddProjectReferenceAsync(string filePath, string projectToReference);

    Task AddPackageReferenceAsync(string filePath, string packageName, string version);

    Task AddImportAsync(string filePath, string importFilePath);

    Task AddAssemblyVersionAsync(string filePath, string version);

    Task AddCopyLocalLockFileAssembliesAsync(string filePath);

    Task ConvertPackageReferenceToProjectReferenceAsync(string filePath, string projectToReference);


    Task<string> GetTargetFrameworkAsync(string filePath);

    Task<List<PackageDependency>> GetDependencyListAsync(string filePath);
}
