using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Authorization.Permissions
{
    public interface IPermissionValueProvider : ISingletonDependency
    {
        string Name { get; }

        //TODO: Rename to GetResult? (CheckAsync throws exception by naming convention)
        Task<PermissionGrantResult> CheckAsync(PermissionValueCheckContext context);
    }
}