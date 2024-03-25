using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Uow;

namespace Volo.Abp.Users;

public abstract class UserLookupService<TUser, TUserRepository> : IUserLookupService<TUser>, ITransientDependency
    where TUser : class, IUser
    where TUserRepository : IUserRepository<TUser>
{
    protected bool SkipExternalLookupIfLocalUserExists { get; set; } = true;

    public IAbpLazyServiceProvider LazyServiceProvider { get; set; } = default!;

    public IExternalUserLookupServiceProvider ExternalUserLookupServiceProvider { get; set; }

    public ILogger<UserLookupService<TUser, TUserRepository>> Logger { get; set; } =
        NullLogger<UserLookupService<TUser, TUserRepository>>.Instance;

    protected TUserRepository UserRepository => LazyServiceProvider.LazyGetRequiredService<TUserRepository>();

    protected IUnitOfWorkManager UnitOfWorkManager => LazyServiceProvider.LazyGetRequiredService<IUnitOfWorkManager>();

    protected EntitySynchronizer<TUser, Guid, UserEto> UserSynchronizer =>
        LazyServiceProvider.LazyGetService<EntitySynchronizer<TUser, Guid, UserEto>>();

    public async Task<TUser> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var localUser = await UserRepository.FindAsync(id, cancellationToken: cancellationToken);

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
                    await WithNewUowAsync(() =>
                        UserRepository.DeleteAsync(localUser, cancellationToken: cancellationToken));
                }

                return null;
            }
        }
        catch (Exception ex)
        {
            Logger.LogException(ex);
            return localUser;
        }

        await CreateOrUpdateLocalUserAsync(externalUser, localUser, cancellationToken);

        return await UserRepository.FindAsync(id, cancellationToken: cancellationToken);
    }

    public async Task<TUser> FindByUserNameAsync(string userName, CancellationToken cancellationToken = default)
    {
        var localUser = await UserRepository.FindByUserNameAsync(userName, cancellationToken);

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
                    await WithNewUowAsync(() =>
                        UserRepository.DeleteAsync(localUser, cancellationToken: cancellationToken));
                }

                return null;
            }
        }
        catch (Exception ex)
        {
            Logger.LogException(ex);
            return localUser;
        }

        await CreateOrUpdateLocalUserAsync(externalUser, localUser, cancellationToken);

        return await UserRepository.FindAsync(externalUser.Id, cancellationToken: cancellationToken);
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

        var localUsers = await UserRepository
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

        return await UserRepository
            .GetCountAsync(
                filter,
                cancellationToken
            );
    }

    protected abstract TUser CreateUser(IUserData externalUser);

    protected virtual async Task CreateOrUpdateLocalUserAsync(IUserData externalUser, [CanBeNull] TUser localUser,
        CancellationToken cancellationToken = default)
    {
        if (UserSynchronizer is not null)
        {
            await UserSynchronizer.TryCreateOrUpdateEntityAsync(externalUser.ToUserEto(), cancellationToken);
        }
        else
        {
            if (localUser == null)
            {
                await WithNewUowAsync(() =>
                    UserRepository.InsertAsync(CreateUser(externalUser), cancellationToken: cancellationToken));
            }

            if (localUser is IUpdateUserData && ((IUpdateUserData)localUser).Update(externalUser))
            {
                await WithNewUowAsync(() =>
                    UserRepository.UpdateAsync(localUser, cancellationToken: cancellationToken));
            }
        }
    }

    private async Task WithNewUowAsync(Func<Task> func)
    {
        using (var uow = UnitOfWorkManager.Begin(requiresNew: true))
        {
            await func();
            await uow.CompleteAsync();
        }
    }
}