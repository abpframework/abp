using System.Threading.Tasks;

namespace Volo.Abp.Studio.Packages.Modifying
{
    public interface ICsprojFileManager
    {
        Task AddProjectReferenceAsync(string filePath, string projectToReference);

        Task AddPackageReferenceAsync(string filePath, string packageName, string version);
    }
}
