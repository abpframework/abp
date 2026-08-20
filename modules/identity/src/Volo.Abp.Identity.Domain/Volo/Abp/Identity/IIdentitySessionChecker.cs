using System.Threading.Tasks;

namespace Volo.Abp.Identity;

public interface IIdentitySessionChecker
{
    Task<bool> IsValidateAsync(string sessionId);
}
