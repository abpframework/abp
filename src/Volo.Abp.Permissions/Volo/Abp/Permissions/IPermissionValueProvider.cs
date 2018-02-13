using System.Threading.Tasks;

namespace Volo.Abp.Permissions
{
    public interface IPermissionValueProvider
    {
        string Name { get; }

        Task<PermissionValueProviderGrantInfo> CheckAsync(PermissionDefinition permission);
    }
}