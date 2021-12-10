using System;
using JetBrains.Annotations;

namespace Volo.Abp.Users;

public interface IUserData
{
    Guid Id { get; }

    Guid? TenantId { get; }

    string UserName { get; }

    string Name { get; }

    string Surname { get; }

    [CanBeNull]
    string Email { get; }

    bool EmailConfirmed { get; }

    [CanBeNull]
    string PhoneNumber { get; }

    bool PhoneNumberConfirmed { get; }
}
