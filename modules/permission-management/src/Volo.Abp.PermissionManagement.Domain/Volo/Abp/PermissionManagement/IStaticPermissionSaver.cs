using System.Threading.Tasks;

namespace Volo.Abp.PermissionManagement;

public interface IStaticPermissionSaver
{
    Task SaveAsync();
}