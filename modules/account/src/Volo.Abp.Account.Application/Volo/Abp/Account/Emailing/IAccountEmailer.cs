using System.Threading.Tasks;
using Volo.Abp.Identity;

namespace Volo.Abp.Account.Emailing
{
    public interface IAccountEmailer
    {
        Task SendPasswordResetLinkAsync(
            IdentityUser user,
            string resetToken,
            string appName,
            string returnUrl = null,
            string returnUrlHash = null
        );
    }
}
