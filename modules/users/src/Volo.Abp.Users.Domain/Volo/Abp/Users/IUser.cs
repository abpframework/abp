using System;
using JetBrains.Annotations;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Users;

public interface IUser : IAggregateRoot<Guid>, IMultiTenant, IHasExtraProperties
{
    string UserName { get; }

    [CanBeNull]
    string Email { get; }

    [CanBeNull]
    string Name { get; }

    [CanBeNull]
    string Surname { get; }

    bool IsActive { get; }

    bool EmailConfirmed { get; }

    [CanBeNull]
    string PhoneNumber { get; }

    bool PhoneNumberConfirmed { get; }
}
