using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace Volo.Abp.Identity;

/// <summary>
/// Represents a new instance of a persistence store for the specified user and role types.
/// </summary>
public class IdentityUserStore :
    IUserLoginStore<IdentityUser>,
    IUserRoleStore<IdentityUser>,
    IUserClaimStore<IdentityUser>,
    IUserPasswordStore<IdentityUser>,
    IUserSecurityStampStore<IdentityUser>,
    IUserEmailStore<IdentityUser>,
    IUserLockoutStore<IdentityUser>,
    IUserPhoneNumberStore<IdentityUser>,
    IUserTwoFactorStore<IdentityUser>,
    IUserAuthenticationTokenStore<IdentityUser>,
    IUserAuthenticatorKeyStore<IdentityUser>,
    IUserTwoFactorRecoveryCodeStore<IdentityUser>,
    ITransientDependency
{
    private const string InternalLoginProvider = "[AspNetUserStore]";
    private const string AuthenticatorKeyTokenName = "AuthenticatorKey";
    private const string RecoveryCodeTokenName = "RecoveryCodes";

    /// <summary>
    /// Gets or sets the <see cref="IdentityErrorDescriber"/> for any error that occurred with the current operation.
    /// </summary>
    public IdentityErrorDescriber ErrorDescriber { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating if changes should be persisted after CreateAsync, UpdateAsync and DeleteAsync are called.
    /// </summary>
    /// <value>
    /// True if changes should be automatically persisted, otherwise false.
    /// </value>
    public bool AutoSaveChanges { get; set; } = true;

    protected IIdentityRoleRepository RoleRepository { get; }
    protected IGuidGenerator GuidGenerator { get; }
    protected ILogger<IdentityRoleStore> Logger { get; }
    protected ILookupNormalizer LookupNormalizer { get; }
    protected IIdentityUserRepository UserRepository { get; }

    public IdentityUserStore(
        IIdentityUserRepository userRepository,
        IIdentityRoleRepository roleRepository,
        IGuidGenerator guidGenerator,
        ILogger<IdentityRoleStore> logger,
        ILookupNormalizer lookupNormalizer,
        IdentityErrorDescriber describer = null)
    {
        UserRepository = userRepository;
        RoleRepository = roleRepository;
        GuidGenerator = guidGenerator;
        Logger = logger;
        LookupNormalizer = lookupNormalizer;

        ErrorDescriber = describer ?? new IdentityErrorDescriber();
    }

    /// <summary>
    /// Gets the user identifier for the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user whose identifier should be retrieved.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the identifier for the specified <paramref name="user"/>.</returns>
    public virtual Task<string> GetUserIdAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        return Task.FromResult(user.Id.ToString());
    }

    /// <summary>
    /// Gets the user name for the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user whose name should be retrieved.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the name for the specified <paramref name="user"/>.</returns>
    public virtual Task<string> GetUserNameAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        return Task.FromResult(user.UserName);
    }

    /// <summary>
    /// Sets the given <paramref name="userName" /> for the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user whose name should be set.</param>
    /// <param name="userName">The user name to set.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public virtual Task SetUserNameAsync([NotNull] IdentityUser user, string userName, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        user.UserName = userName;

        return Task.CompletedTask;
    }

    /// <summary>
    /// Gets the normalized user name for the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user whose normalized name should be retrieved.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the normalized user name for the specified <paramref name="user"/>.</returns>
    public virtual Task<string> GetNormalizedUserNameAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        return Task.FromResult(user.NormalizedUserName);
    }

    /// <summary>
    /// Sets the given normalized name for the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user whose name should be set.</param>
    /// <param name="normalizedName">The normalized name to set.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public virtual Task SetNormalizedUserNameAsync([NotNull] IdentityUser user, string normalizedName, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        user.NormalizedUserName = normalizedName;

        return Task.CompletedTask;
    }

    /// <summary>
    /// Creates the specified <paramref name="user"/> in the user store.
    /// </summary>
    /// <param name="user">The user to create.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the creation operation.</returns>
    public virtual async Task<IdentityResult> CreateAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        await UserRepository.InsertAsync(user, AutoSaveChanges, cancellationToken);

        return IdentityResult.Success;
    }

    /// <summary>
    /// Updates the specified <paramref name="user"/> in the user store.
    /// </summary>
    /// <param name="user">The user to update.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the update operation.</returns>
    public virtual async Task<IdentityResult> UpdateAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        try
        {
            await UserRepository.UpdateAsync(user, AutoSaveChanges, cancellationToken);
        }
        catch (AbpDbConcurrencyException ex)
        {
            Logger.LogWarning(ex.ToString()); //Trigger some AbpHandledException event
            return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
        }

        return IdentityResult.Success;
    }

    /// <summary>
    /// Deletes the specified <paramref name="user"/> from the user store.
    /// </summary>
    /// <param name="user">The user to delete.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the update operation.</returns>
    public virtual async Task<IdentityResult> DeleteAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        try
        {
            await UserRepository.DeleteAsync(user, AutoSaveChanges, cancellationToken);
        }
        catch (AbpDbConcurrencyException ex)
        {
            Logger.LogWarning(ex.ToString()); //Trigger some AbpHandledException event
            return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
        }

        return IdentityResult.Success;
    }

    /// <summary>
    /// Finds and returns a user, if any, who has the specified <paramref name="userId"/>.
    /// </summary>
    /// <param name="userId">The user ID to search for.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing the user matching the specified <paramref name="userId"/> if it exists.
    /// </returns>
    public virtual Task<IdentityUser> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return UserRepository.FindAsync(Guid.Parse(userId), cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Finds and returns a user, if any, who has the specified normalized user name.
    /// </summary>
    /// <param name="normalizedUserName">The normalized user name to search for.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing the user matching the specified <paramref name="normalizedUserName"/> if it exists.
    /// </returns>
    public virtual Task<IdentityUser> FindByNameAsync([NotNull] string normalizedUserName, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(normalizedUserName, nameof(normalizedUserName));

        return UserRepository.FindByNormalizedUserNameAsync(normalizedUserName, includeDetails: false, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Sets the password hash for a user.
    /// </summary>
    /// <param name="user">The user to set the password hash for.</param>
    /// <param name="passwordHash">The password hash to set.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public virtual Task SetPasswordHashAsync([NotNull] IdentityUser user, string passwordHash, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        user.PasswordHash = passwordHash;

        return Task.CompletedTask;
    }

    /// <summary>
    /// Gets the password hash for a user.
    /// </summary>
    /// <param name="user">The user to retrieve the password hash for.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>A <see cref="Task{TResult}"/> that contains the password hash for the user.</returns>
    public virtual Task<string> GetPasswordHashAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        return Task.FromResult(user.PasswordHash);
    }

    /// <summary>
    /// Returns a flag indicating if the specified user has a password.
    /// </summary>
    /// <param name="user">The user to retrieve the password hash for.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>A <see cref="Task{TResult}"/> containing a flag indicating if the specified user has a password. If the
    /// user has a password the returned value with be true, otherwise it will be false.</returns>
    public virtual Task<bool> HasPasswordAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        return Task.FromResult(user.PasswordHash != null);
    }

    /// <summary>
    /// Adds the given <paramref name="normalizedRoleName"/> to the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user to add the role to.</param>
    /// <param name="normalizedRoleName">The role to add.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public virtual async Task AddToRoleAsync([NotNull] IdentityUser user, [NotNull] string normalizedRoleName, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));
        Check.NotNull(normalizedRoleName, nameof(normalizedRoleName));

        if (await IsInRoleAsync(user, normalizedRoleName, cancellationToken))
        {
            return;
        }

        var role = await RoleRepository.FindByNormalizedNameAsync(normalizedRoleName, cancellationToken: cancellationToken);
        if (role == null)
        {
            throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Role {0} does not exist!", normalizedRoleName));
        }

        await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Roles, cancellationToken);

        user.AddRole(role.Id);
    }

    /// <summary>
    /// Removes the given <paramref name="normalizedRoleName"/> from the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user to remove the role from.</param>
    /// <param name="normalizedRoleName">The role to remove.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public virtual async Task RemoveFromRoleAsync([NotNull] IdentityUser user, [NotNull] string normalizedRoleName, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));
        Check.NotNullOrWhiteSpace(normalizedRoleName, nameof(normalizedRoleName));

        var role = await RoleRepository.FindByNormalizedNameAsync(normalizedRoleName, cancellationToken: cancellationToken);
        if (role == null)
        {
            return;
        }

        await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Roles, cancellationToken);

        user.RemoveRole(role.Id);
    }

    /// <summary>
    /// Retrieves the roles the specified <paramref name="user"/> is a member of.
    /// </summary>
    /// <param name="user">The user whose roles should be retrieved.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>A <see cref="Task{TResult}"/> that contains the roles the user is a member of.</returns>
    public virtual async Task<IList<string>> GetRolesAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));
        //Returns the roles owned by the current user
        var userRoles = await UserRepository
            .GetRoleNamesAsync(user.Id, cancellationToken: cancellationToken);
        return userRoles;

        /*var userOrganizationUnitRoles = await UserRepository
            .GetRoleNamesInOrganizationUnitAsync(user.Id, cancellationToken: cancellationToken);

        return userRoles.Union(userOrganizationUnitRoles).ToList();*/
    }

    /// <summary>
    /// Returns a flag indicating if the specified user is a member of the give <paramref name="normalizedRoleName"/>.
    /// </summary>
    /// <param name="user">The user whose role membership should be checked.</param>
    /// <param name="normalizedRoleName">The role to check membership of</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>A <see cref="Task{TResult}"/> containing a flag indicating if the specified user is a member of the given group. If the
    /// user is a member of the group the returned value with be true, otherwise it will be false.</returns>
    public virtual async Task<bool> IsInRoleAsync(
        [NotNull] IdentityUser user,
        [NotNull] string normalizedRoleName,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));
        Check.NotNullOrWhiteSpace(normalizedRoleName, nameof(normalizedRoleName));

        var roles = await GetRolesAsync(user, cancellationToken);

        return roles
            .Select(r => LookupNormalizer.NormalizeName(r))
            .Contains(normalizedRoleName);
    }

    /// <summary>
    /// Get the claims associated with the specified <paramref name="user"/> as an asynchronous operation.
    /// </summary>
    /// <param name="user">The user whose claims should be retrieved.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>A <see cref="Task{TResult}"/> that contains the claims granted to a user.</returns>
    public virtual async Task<IList<Claim>> GetClaimsAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Claims, cancellationToken);

        return user.Claims.Select(c => c.ToClaim()).ToList();
    }

    /// <summary>
    /// Adds the <paramref name="claims"/> given to the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user to add the claim to.</param>
    /// <param name="claims">The claim to add to the user.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public virtual async Task AddClaimsAsync([NotNull] IdentityUser user, [NotNull] IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));
        Check.NotNull(claims, nameof(claims));

        await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Claims, cancellationToken);

        user.AddClaims(GuidGenerator, claims);
    }

    /// <summary>
    /// Replaces the <paramref name="claim"/> on the specified <paramref name="user"/>, with the <paramref name="newClaim"/>.
    /// </summary>
    /// <param name="user">The user to replace the claim on.</param>
    /// <param name="claim">The claim replace.</param>
    /// <param name="newClaim">The new claim replacing the <paramref name="claim"/>.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public virtual async Task ReplaceClaimAsync([NotNull] IdentityUser user, [NotNull] Claim claim, [NotNull] Claim newClaim, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));
        Check.NotNull(claim, nameof(claim));
        Check.NotNull(newClaim, nameof(newClaim));

        await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Claims, cancellationToken);

        user.ReplaceClaim(claim, newClaim);
    }

    /// <summary>
    /// Removes the <paramref name="claims"/> given from the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user to remove the claims from.</param>
    /// <param name="claims">The claim to remove.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public virtual async Task RemoveClaimsAsync([NotNull] IdentityUser user, [NotNull] IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));
        Check.NotNull(claims, nameof(claims));

        await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Claims, cancellationToken);

        user.RemoveClaims(claims);
    }

    /// <summary>
    /// Adds the <paramref name="login"/> given to the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user to add the login to.</param>
    /// <param name="login">The login to add to the user.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public virtual async Task AddLoginAsync([NotNull] IdentityUser user, [NotNull] UserLoginInfo login, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));
        Check.NotNull(login, nameof(login));

        await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Logins, cancellationToken);

        user.AddLogin(login);
    }

    /// <summary>
    /// Removes the <paramref name="loginProvider"/> given from the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user to remove the login from.</param>
    /// <param name="loginProvider">The login to remove from the user.</param>
    /// <param name="providerKey">The key provided by the <paramref name="loginProvider"/> to identify a user.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public virtual async Task RemoveLoginAsync([NotNull] IdentityUser user, [NotNull] string loginProvider, [NotNull] string providerKey, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));
        Check.NotNull(loginProvider, nameof(loginProvider));
        Check.NotNull(providerKey, nameof(providerKey));

        await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Logins, cancellationToken);

        user.RemoveLogin(loginProvider, providerKey);
    }

    /// <summary>
    /// Retrieves the associated logins for the specified <param ref="user"/>.
    /// </summary>
    /// <param name="user">The user whose associated logins to retrieve.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>
    /// The <see cref="Task"/> for the asynchronous operation, containing a list of <see cref="UserLoginInfo"/> for the specified <paramref name="user"/>, if any.
    /// </returns>
    public virtual async Task<IList<UserLoginInfo>> GetLoginsAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Logins, cancellationToken);

        return user.Logins.Select(l => l.ToUserLoginInfo()).ToList();
    }

    /// <summary>
    /// Retrieves the user associated with the specified login provider and login provider key..
    /// </summary>
    /// <param name="loginProvider">The login provider who provided the <paramref name="providerKey"/>.</param>
    /// <param name="providerKey">The key provided by the <paramref name="loginProvider"/> to identify a user.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>
    /// The <see cref="Task"/> for the asynchronous operation, containing the user, if any which matched the specified login provider and key.
    /// </returns>
    public virtual Task<IdentityUser> FindByLoginAsync([NotNull] string loginProvider, [NotNull] string providerKey, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(loginProvider, nameof(loginProvider));
        Check.NotNull(providerKey, nameof(providerKey));

        return UserRepository.FindByLoginAsync(loginProvider, providerKey, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Gets a flag indicating whether the email address for the specified <paramref name="user"/> has been verified, true if the email address is verified otherwise
    /// false.
    /// </summary>
    /// <param name="user">The user whose email confirmation status should be returned.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>
    /// The task object containing the results of the asynchronous operation, a flag indicating whether the email address for the specified <paramref name="user"/>
    /// has been confirmed or not.
    /// </returns>
    public virtual Task<bool> GetEmailConfirmedAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        return Task.FromResult(user.EmailConfirmed);
    }

    /// <summary>
    /// Sets the flag indicating whether the specified <paramref name="user"/>'s email address has been confirmed or not.
    /// </summary>
    /// <param name="user">The user whose email confirmation status should be set.</param>
    /// <param name="confirmed">A flag indicating if the email address has been confirmed, true if the address is confirmed otherwise false.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public virtual Task SetEmailConfirmedAsync([NotNull] IdentityUser user, bool confirmed, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        user.SetEmailConfirmed(confirmed);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Sets the <paramref name="email"/> address for a <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user whose email should be set.</param>
    /// <param name="email">The email to set.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public virtual Task SetEmailAsync([NotNull] IdentityUser user, [NotNull] string email, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));
        Check.NotNull(email, nameof(email));

        user.Email = email;

        return Task.CompletedTask;
    }

    /// <summary>
    /// Gets the email address for the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user whose email should be returned.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The task object containing the results of the asynchronous operation, the email address for the specified <paramref name="user"/>.</returns>
    public virtual Task<string> GetEmailAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        return Task.FromResult(user.Email);
    }

    /// <summary>
    /// Returns the normalized email for the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user whose email address to retrieve.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>
    /// The task object containing the results of the asynchronous lookup operation, the normalized email address if any associated with the specified user.
    /// </returns>
    public virtual Task<string> GetNormalizedEmailAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        return Task.FromResult(user.NormalizedEmail);
    }

    /// <summary>
    /// Sets the normalized email for the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user whose email address to set.</param>
    /// <param name="normalizedEmail">The normalized email to set for the specified <paramref name="user"/>.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public virtual Task SetNormalizedEmailAsync([NotNull] IdentityUser user, string normalizedEmail, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        user.NormalizedEmail = normalizedEmail;

        return Task.CompletedTask;
    }

    /// <summary>
    /// Gets the user, if any, associated with the specified, normalized email address.
    /// </summary>
    /// <param name="normalizedEmail">The normalized email address to return the user for.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>
    /// The task object containing the results of the asynchronous lookup operation, the user if any associated with the specified normalized email address.
    /// </returns>
    public virtual Task<IdentityUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return UserRepository.FindByNormalizedEmailAsync(normalizedEmail, includeDetails: false, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Gets the last <see cref="DateTimeOffset"/> a user's last lockout expired, if any.
    /// Any time in the past should be indicates a user is not locked out.
    /// </summary>
    /// <param name="user">The user whose lockout date should be retrieved.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> that represents the result of the asynchronous query, a <see cref="DateTimeOffset"/> containing the last time
    /// a user's lockout expired, if any.
    /// </returns>
    public virtual Task<DateTimeOffset?> GetLockoutEndDateAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        return Task.FromResult(user.LockoutEnd);
    }

    /// <summary>
    /// Locks out a user until the specified end date has passed. Setting a end date in the past immediately unlocks a user.
    /// </summary>
    /// <param name="user">The user whose lockout date should be set.</param>
    /// <param name="lockoutEnd">The <see cref="DateTimeOffset"/> after which the <paramref name="user"/>'s lockout should end.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public virtual Task SetLockoutEndDateAsync([NotNull] IdentityUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        user.LockoutEnd = lockoutEnd;

        return Task.CompletedTask;
    }

    /// <summary>
    /// Records that a failed access has occurred, incrementing the failed access count.
    /// </summary>
    /// <param name="user">The user whose cancellation count should be incremented.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the incremented failed access count.</returns>
    public virtual Task<int> IncrementAccessFailedCountAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        user.AccessFailedCount++;

        return Task.FromResult(user.AccessFailedCount);
    }

    /// <summary>
    /// Resets a user's failed access count.
    /// </summary>
    /// <param name="user">The user whose failed access count should be reset.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    /// <remarks>This is typically called after the account is successfully accessed.</remarks>
    public virtual Task ResetAccessFailedCountAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        user.AccessFailedCount = 0;

        return Task.CompletedTask;
    }

    /// <summary>
    /// Retrieves the current failed access count for the specified <paramref name="user"/>..
    /// </summary>
    /// <param name="user">The user whose failed access count should be retrieved.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the failed access count.</returns>
    public virtual Task<int> GetAccessFailedCountAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        return Task.FromResult(user.AccessFailedCount);
    }

    /// <summary>
    /// Retrieves a flag indicating whether user lockout can enabled for the specified user.
    /// </summary>
    /// <param name="user">The user whose ability to be locked out should be returned.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, true if a user can be locked out, otherwise false.
    /// </returns>
    public virtual Task<bool> GetLockoutEnabledAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        return Task.FromResult(user.LockoutEnabled);
    }

    /// <summary>
    /// Set the flag indicating if the specified <paramref name="user"/> can be locked out..
    /// </summary>
    /// <param name="user">The user whose ability to be locked out should be set.</param>
    /// <param name="enabled">A flag indicating if lock out can be enabled for the specified <paramref name="user"/>.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public virtual Task SetLockoutEnabledAsync([NotNull] IdentityUser user, bool enabled, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        user.LockoutEnabled = enabled;

        return Task.CompletedTask;
    }

    /// <summary>
    /// Sets the telephone number for the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user whose telephone number should be set.</param>
    /// <param name="phoneNumber">The telephone number to set.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public virtual Task SetPhoneNumberAsync([NotNull] IdentityUser user, string phoneNumber, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        user.PhoneNumber = phoneNumber;

        return Task.CompletedTask;
    }

    /// <summary>
    /// Gets the telephone number, if any, for the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user whose telephone number should be retrieved.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the user's telephone number, if any.</returns>
    public virtual Task<string> GetPhoneNumberAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        return Task.FromResult(user.PhoneNumber);
    }

    /// <summary>
    /// Gets a flag indicating whether the specified <paramref name="user"/>'s telephone number has been confirmed.
    /// </summary>
    /// <param name="user">The user to return a flag for, indicating whether their telephone number is confirmed.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, returning true if the specified <paramref name="user"/> has a confirmed
    /// telephone number otherwise false.
    /// </returns>
    public virtual Task<bool> GetPhoneNumberConfirmedAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        return Task.FromResult(user.PhoneNumberConfirmed);
    }

    /// <summary>
    /// Sets a flag indicating if the specified <paramref name="user"/>'s phone number has been confirmed..
    /// </summary>
    /// <param name="user">The user whose telephone number confirmation status should be set.</param>
    /// <param name="confirmed">A flag indicating whether the user's telephone number has been confirmed.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public virtual Task SetPhoneNumberConfirmedAsync([NotNull] IdentityUser user, bool confirmed, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        user.SetPhoneNumberConfirmed(confirmed);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Sets the provided security <paramref name="stamp"/> for the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user whose security stamp should be set.</param>
    /// <param name="stamp">The security stamp to set.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public virtual Task SetSecurityStampAsync([NotNull] IdentityUser user, string stamp, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        user.SecurityStamp = stamp;

        return Task.CompletedTask;
    }

    /// <summary>
    /// Get the security stamp for the specified <paramref name="user" />.
    /// </summary>
    /// <param name="user">The user whose security stamp should be set.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the security stamp for the specified <paramref name="user"/>.</returns>
    public virtual Task<string> GetSecurityStampAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        return Task.FromResult(user.SecurityStamp);
    }

    /// <summary>
    /// Sets a flag indicating whether the specified <paramref name="user"/> has two factor authentication enabled or not,
    /// as an asynchronous operation.
    /// </summary>
    /// <param name="user">The user whose two factor authentication enabled status should be set.</param>
    /// <param name="enabled">A flag indicating whether the specified <paramref name="user"/> has two factor authentication enabled.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public virtual Task SetTwoFactorEnabledAsync([NotNull] IdentityUser user, bool enabled, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        user.TwoFactorEnabled = enabled;

        return Task.CompletedTask;
    }

    /// <summary>
    /// Returns a flag indicating whether the specified <paramref name="user"/> has two factor authentication enabled or not,
    /// as an asynchronous operation.
    /// </summary>
    /// <param name="user">The user whose two factor authentication enabled status should be set.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing a flag indicating whether the specified
    /// <paramref name="user"/> has two factor authentication enabled or not.
    /// </returns>
    public virtual Task<bool> GetTwoFactorEnabledAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        return Task.FromResult(user.TwoFactorEnabled);
    }

    /// <summary>
    /// Retrieves all users with the specified claim.
    /// </summary>
    /// <param name="claim">The claim whose users should be retrieved.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>
    /// The <see cref="Task"/> contains a list of users, if any, that contain the specified claim.
    /// </returns>
    public virtual async Task<IList<IdentityUser>> GetUsersForClaimAsync([NotNull] Claim claim, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(claim, nameof(claim));

        return await UserRepository.GetListByClaimAsync(claim, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Retrieves all users in the specified role.
    /// </summary>
    /// <param name="normalizedRoleName">The role whose users should be retrieved.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>
    /// The <see cref="Task"/> contains a list of users, if any, that are in the specified role.
    /// </returns>
    public virtual async Task<IList<IdentityUser>> GetUsersInRoleAsync([NotNull] string normalizedRoleName, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrEmpty(normalizedRoleName))
        {
            throw new ArgumentNullException(nameof(normalizedRoleName));
        }

        return await UserRepository.GetListByNormalizedRoleNameAsync(normalizedRoleName, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Sets the token value for a particular user.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="loginProvider">The authentication provider for the token.</param>
    /// <param name="name">The name of the token.</param>
    /// <param name="value">The value of the token.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public virtual async Task SetTokenAsync([NotNull] IdentityUser user, string loginProvider, string name, string value, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Tokens, cancellationToken);

        user.SetToken(loginProvider, name, value);
    }

    /// <summary>
    /// Deletes a token for a user.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="loginProvider">The authentication provider for the token.</param>
    /// <param name="name">The name of the token.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public virtual async Task RemoveTokenAsync(IdentityUser user, string loginProvider, string name, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Tokens, cancellationToken);

        user.RemoveToken(loginProvider, name);
    }

    /// <summary>
    /// Returns the token value.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="loginProvider">The authentication provider for the token.</param>
    /// <param name="name">The name of the token.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public virtual async Task<string> GetTokenAsync(IdentityUser user, string loginProvider, string name, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Tokens, cancellationToken);

        return user.FindToken(loginProvider, name)?.Value;
    }

    public virtual Task SetAuthenticatorKeyAsync(IdentityUser user, string key, CancellationToken cancellationToken = default)
    {
        return SetTokenAsync(user, InternalLoginProvider, AuthenticatorKeyTokenName, key, cancellationToken);
    }

    public virtual Task<string> GetAuthenticatorKeyAsync(IdentityUser user, CancellationToken cancellationToken = default)
    {
        return GetTokenAsync(user, InternalLoginProvider, AuthenticatorKeyTokenName, cancellationToken);
    }

    /// <summary>
    /// Returns how many recovery code are still valid for a user.
    /// </summary>
    /// <param name="user">The user who owns the recovery code.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The number of valid recovery codes for the user..</returns>
    public virtual async Task<int> CountCodesAsync(IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        var mergedCodes = await GetTokenAsync(user, InternalLoginProvider, RecoveryCodeTokenName, cancellationToken) ?? "";
        if (mergedCodes.Length > 0)
        {
            return mergedCodes.Split(';').Length;
        }

        return 0;
    }

    /// <summary>
    /// Updates the recovery codes for the user while invalidating any previous recovery codes.
    /// </summary>
    /// <param name="user">The user to store new recovery codes for.</param>
    /// <param name="recoveryCodes">The new recovery codes for the user.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The new recovery codes for the user.</returns>
    public virtual Task ReplaceCodesAsync(IdentityUser user, IEnumerable<string> recoveryCodes, CancellationToken cancellationToken = default)
    {
        var mergedCodes = string.Join(";", recoveryCodes);
        return SetTokenAsync(user, InternalLoginProvider, RecoveryCodeTokenName, mergedCodes, cancellationToken);
    }

    /// <summary>
    /// Returns whether a recovery code is valid for a user. Note: recovery codes are only valid
    /// once, and will be invalid after use.
    /// </summary>
    /// <param name="user">The user who owns the recovery code.</param>
    /// <param name="code">The recovery code to use.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>True if the recovery code was found for the user.</returns>
    public virtual async Task<bool> RedeemCodeAsync(IdentityUser user, string code, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));
        Check.NotNull(code, nameof(code));

        var mergedCodes = await GetTokenAsync(user, InternalLoginProvider, RecoveryCodeTokenName, cancellationToken) ?? "";
        var splitCodes = mergedCodes.Split(';');
        if (splitCodes.Contains(code))
        {
            var updatedCodes = new List<string>(splitCodes.Where(s => s != code));
            await ReplaceCodesAsync(user, updatedCodes, cancellationToken);
            return true;
        }
        return false;
    }

    public virtual void Dispose()
    {

    }
}
