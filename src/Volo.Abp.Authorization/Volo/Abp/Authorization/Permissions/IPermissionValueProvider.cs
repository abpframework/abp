using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Authorization.Permissions
{
    public interface IPermissionValueProvider : ISingletonDependency
    {
        string Name { get; }

        Task<PermissionValueProviderGrantInfo> CheckAsync(PermissionDefinition permission);
    }
}