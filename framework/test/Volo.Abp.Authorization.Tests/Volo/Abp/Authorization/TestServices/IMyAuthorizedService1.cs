using System.Threading.Tasks;

namespace Volo.Abp.Authorization.TestServices
{
    public interface IMyAuthorizedService1
    {
        int Anonymous();

        Task<int> AnonymousAsync();

        int ProtectedByClass();

        Task<int> ProtectedByClassAsync();
    }
}