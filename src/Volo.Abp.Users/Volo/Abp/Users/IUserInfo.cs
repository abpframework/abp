using System;
using JetBrains.Annotations;

namespace Volo.Abp.Users
{
    public interface IUserInfo
    {
        Guid Id { get; }

        string UserName { get; }

        [CanBeNull]
        string Email { get; }

        bool EmailConfirmed { get; }

        [CanBeNull]
        string PhoneNumber { get; }

        bool PhoneNumberConfirmed { get; }
    }
}