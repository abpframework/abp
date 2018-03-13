using System;
using JetBrains.Annotations;

namespace Volo.Abp.Users
{
    public class UserInfo : IUserInfo
    {
        public Guid Id { get; }

        public string UserName { get; }

        public string Email { get; }

        public bool EmailConfirmed { get; }

        public string PhoneNumber { get; }

        public bool PhoneNumberConfirmed { get; }

        public UserInfo(
            Guid id,
            [NotNull] string userName,
            [CanBeNull] string email = null,
            bool emailConfirmed = false,
            [CanBeNull] string phoneNumber = null,
            bool phoneNumberConfirmed = false)
        {
            Check.NotNull(userName, nameof(userName));

            Id = id;
            UserName = userName;
            Email = email;
            EmailConfirmed = emailConfirmed;
            PhoneNumber = phoneNumber;
            PhoneNumberConfirmed = phoneNumberConfirmed;
        }
    }
}