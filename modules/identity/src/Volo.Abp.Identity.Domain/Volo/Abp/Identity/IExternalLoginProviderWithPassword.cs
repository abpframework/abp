using System.Threading.Tasks;

namespace Volo.Abp.Identity
{
    public interface IExternalLoginProviderWithPassword
    {
        bool CanObtainUserInfoWithoutPassword { get; }
        
        /// <summary>
        /// This method is called when a user is authenticated by this source but the user does not exists yet.
        /// So, the source should create the user and fill the properties.
        /// </summary>
        /// <param name="userName">User name</param>
        /// <param name="providerName">The name of this provider</param>
        /// <param name="plainPassword">The plain password of the user</param>
        /// <returns>Newly created user</returns>
        Task<IdentityUser> CreateUserAsync(string userName, string providerName, string plainPassword);

        /// <summary>
        /// This method is called after an existing user is authenticated by this source.
        /// It can be used to update some properties of the user by the source.
        /// </summary>
        /// <param name="providerName">The name of this provider</param>
        /// <param name="user">The user that can be updated</param>
        /// <param name="plainPassword">The plain password of the user</param>
        Task UpdateUserAsync(IdentityUser user, string providerName, string plainPassword);
    }
}