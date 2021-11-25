using System.Threading.Tasks;

namespace Volo.Abp.Authorization.TestServices;

public interface IMySimpleAuthorizedService
{
    Task<int> ProtectedByClassAsync();

    Task<int> AnonymousAsync();
}
