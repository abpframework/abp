using System;
using JetBrains.Annotations;
using Volo.Abp.Auditing;
using Volo.Abp.Data;

namespace Volo.Abp.Users;

public interface IUserData : IHasExtraProperties, IHasEntityVersion
{
    Guid Id { get; }

    Guid? TenantId { get; }

    string UserName { get; }

    string Name { get; }

    string Surname { get; }

    bool IsActive { get; }

    [CanBeNull]
    string Email { get; }

    bool EmailConfirmed { get; }

    [CanBeNull]
    string PhoneNumber { get; }

    bool PhoneNumberConfirmed { get; }
}
