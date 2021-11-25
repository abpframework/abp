using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace Volo.Abp.Users;

public abstract class UserLookupService<TUser, TUserRepository> : IUserLookupService<TUser>, ITransientDependency
    where TUser : class, IUser
    where TUserRepository : IUserRepository<TUser>
{
    protected bool SkipExternalLookupIfLocalUserExists { get; set; } = true;

    public IExternalUserLookupServiceProvider ExternalUserLookupServiceProvider { get; set; }
    public ILogger<UserLookupService<TUser, TUserRepository>> Logger { get; set; }

    private readonly TUserRepository _userRepository;
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    protected UserLookupService(
        TUserRepository userRepository,
        IUnitOfWorkManager unitOfWorkManager)
    {
        _userRepository = userRepository;
        _unitOfWorkManager = unitOfWorkManager;

        Logger = NullLogger<UserLookupService<TUser, TUserRepository>>.Instance;
    }

    public async Task<TUser> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var localUser = await _userRepository.FindAsync(id, cancellationToken: cancellationToken);

        if (ExternalUserLookupServiceProvider == null)
        {
            return localUser;
        }

        if (SkipExternalLookupIfLocalUserExists && localUser != null)
        {
            return localUser;
        }

        IUserData externalUser;

        try
        {
            externalUser = await ExternalUserLookupServiceProvider.FindByIdAsync(id, cancellationToken);
            if (externalUser == null)
            {
                if (localUser != null)
                {
                    //TODO: Instead of deleting, should be make it inactive or something like that?
                    await WithNewUowAsync(() => _userRepository.DeleteAsync(localUser, cancellationToken: cancellationToken));
                }

                return null;
            }
        }
        catch (Exception ex)
        {
            Logger.LogException(ex);
            return localUser;
        }

        if (localUser == null)
        {
            await WithNewUowAsync(() => _userRepository.InsertAsync(CreateUser(externalUser), cancellationToken: cancellationToken));
            return await _userRepository.FindAsync(id, cancellationToken: cancellationToken);
        }

        if (localUser is IUpdateUserData && ((IUpdateUserData)localUser).Update(externalUser))
        {
            await WithNewUowAsync(() => _userRepository.UpdateAsync(localUser, cancellationToken: cancellationToken));
        }
        else
        {
            return localUser;
        }

        return await _userRepository.FindAsync(id, cancellationToken: cancellationToken);
    }

    public async Task<TUser> FindByUserNameAsync(string userName, CancellationToken cancellationToken = default)
    {
        var localUser = await _userRepository.FindByUserNameAsync(userName, cancellationToken);

        if (ExternalUserLookupServiceProvider == null)
        {
            return localUser;
        }

        if (SkipExternalLookupIfLocalUserExists && localUser != null)
        {
            return localUser;
        }

        IUserData externalUser;

        try
        {
            externalUser = await ExternalUserLookupServiceProvider.FindByUserNameAsync(userName, cancellationToken);
            if (externalUser == null)
            {
                if (localUser != null)
                {
                    //TODO: Instead of deleting, should be make it passive or something like that?
                    await WithNewUowAsync(() => _userRepository.DeleteAsync(localUser, cancellationToken: cancellationToken));
                }

                return null;
            }
        }
        catch (Exception ex)
        {
            Logger.LogException(ex);
            return localUser;
        }

        if (localUser == null)
        {
            await WithNewUowAsync(() => _userRepository.InsertAsync(CreateUser(externalUser), cancellationToken: cancellationToken));
            return await _userRepository.FindAsync(externalUser.Id, cancellationToken: cancellationToken);
        }

        if (localUser is IUpdateUserData && ((IUpdateUserData)localUser).Update(externalUser))
        {
            await WithNewUowAsync(() => _userRepository.UpdateAsync(localUser, cancellationToken: cancellationToken));
        }
        else
        {
            return localUser;
        }

        return await _userRepository.FindAsync(externalUser.Id, cancellationToken: cancellationToken);
    }

    public async Task<List<IUserData>> SearchAsync(
        string sorting = null,
        string filter = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        if (ExternalUserLookupServiceProvider != null)
        {
            return await ExternalUserLookupServiceProvider
                .SearchAsync(
                    sorting,
                    filter,
                    maxResultCount,
                    skipCount,
                    cancellationToken
                );
        }

        var localUsers = await _userRepository
            .SearchAsync(
                sorting,
                maxResultCount,
                skipCount,
                filter,
                cancellationToken
            );

        return localUsers
            .Cast<IUserData>()
            .ToList();
    }

    public async Task<long> GetCountAsync(
        string filter = null,
        CancellationToken cancellationToken = default)
    {
        if (ExternalUserLookupServiceProvider != null)
        {
            return await ExternalUserLookupServiceProvider
                .GetCountAsync(
                    filter,
                    cancellationToken
                );
        }

        return await _userRepository
            .GetCountAsync(
                filter,
                cancellationToken
            );
    }

    protected abstract TUser CreateUser(IUserData externalUser);

    private async Task WithNewUowAsync(Func<Task> func)
    {
        using (var uow = _unitOfWorkManager.Begin(requiresNew: true))
        {
            await func();
            await uow.CompleteAsync();
        }
    }
}
