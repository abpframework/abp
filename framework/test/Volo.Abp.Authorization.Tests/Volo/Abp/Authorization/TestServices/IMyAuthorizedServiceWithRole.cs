using System.Threading.Tasks;

namespace Volo.Abp.Authorization.TestServices
{
    public interface IMyAuthorizedServiceWithRole
    {
        Task<int> ProtectedByRole();

        Task<int> ProtectedByScheme();

        Task<int> ProtectedByAnotherRole();
    }
}
