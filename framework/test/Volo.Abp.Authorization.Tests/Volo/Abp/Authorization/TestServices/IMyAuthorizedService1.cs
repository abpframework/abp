using System.Threading.Tasks;

namespace Volo.Abp.Authorization.TestServices
{
    public interface IMyAuthorizedService1
    {
        Task<int> Anonymous();

        Task<int> AnonymousAsync();

        Task<int> ProtectedByClass();

        Task<int> ProtectedByClassAsync();

        Task<int> ProtectedByRole();

        Task<int> ProtectedByScheme();
    }
}