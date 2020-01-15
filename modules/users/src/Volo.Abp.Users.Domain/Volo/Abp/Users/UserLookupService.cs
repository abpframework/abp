﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace Volo.Abp.Users
{
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
            var localUser = await _userRepository.FindAsync(id, cancellationToken: cancellationToken).ConfigureAwait(false);

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
                externalUser = await ExternalUserLookupServiceProvider.FindByIdAsync(id, cancellationToken).ConfigureAwait(false);
                if (externalUser == null)
                {
                    if (localUser != null)
                    {
                        //TODO: Instead of deleting, should be make it inactive or something like that?
                        await WithNewUowAsync(() => _userRepository.DeleteAsync(localUser, cancellationToken: cancellationToken)).ConfigureAwait(false);
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
                await WithNewUowAsync(() => _userRepository.InsertAsync(CreateUser(externalUser), cancellationToken: cancellationToken)).ConfigureAwait(false);
                return await _userRepository.FindAsync(id, cancellationToken: cancellationToken).ConfigureAwait(false);
            }

            if (localUser is IUpdateUserData && ((IUpdateUserData)localUser).Update(externalUser))
            {
                await WithNewUowAsync(() => _userRepository.UpdateAsync(localUser, cancellationToken: cancellationToken)).ConfigureAwait(false);
            }
            else
            {
                return localUser;
            }

            return await _userRepository.FindAsync(id, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<TUser> FindByUserNameAsync(string userName, CancellationToken cancellationToken = default)
        {
            var localUser = await _userRepository.FindByUserNameAsync(userName, cancellationToken).ConfigureAwait(false);

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
                externalUser = await ExternalUserLookupServiceProvider.FindByUserNameAsync(userName, cancellationToken).ConfigureAwait(false);
                if (externalUser == null)
                {
                    if (localUser != null)
                    {
                        //TODO: Instead of deleting, should be make it passive or something like that?
                        await WithNewUowAsync(() => _userRepository.DeleteAsync(localUser, cancellationToken: cancellationToken)).ConfigureAwait(false);
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
                await WithNewUowAsync(() => _userRepository.InsertAsync(CreateUser(externalUser), cancellationToken: cancellationToken)).ConfigureAwait(false);
                return await _userRepository.FindAsync(externalUser.Id, cancellationToken: cancellationToken).ConfigureAwait(false);
            }

            if (localUser is IUpdateUserData && ((IUpdateUserData)localUser).Update(externalUser))
            {
                await WithNewUowAsync(() => _userRepository.UpdateAsync(localUser, cancellationToken: cancellationToken)).ConfigureAwait(false);
            }
            else
            {
                return localUser;
            }

            return await _userRepository.FindAsync(externalUser.Id, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        protected abstract TUser CreateUser(IUserData externalUser);

        private async Task WithNewUowAsync(Func<Task> func)
        {
            using (var uow = _unitOfWorkManager.Begin(requiresNew: true))
            {
                await func().ConfigureAwait(false);
                await uow.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}