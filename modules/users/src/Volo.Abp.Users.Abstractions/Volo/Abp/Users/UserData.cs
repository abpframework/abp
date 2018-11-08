using System;
using JetBrains.Annotations;

namespace Volo.Abp.Users
{
    public class UserData : IUserData
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public Guid? TenantId { get; set; }

        public UserData()
        {

        }

        public UserData(
            Guid id,
            [NotNull] string userName,
            [CanBeNull] string email = null,
            [CanBeNull] string name = null,
            [CanBeNull] string surname = null,
            bool emailConfirmed = false,
            [CanBeNull] string phoneNumber = null,
            bool phoneNumberConfirmed = false,
            Guid? tenantId = null)
        {
            Id = id;
            UserName = userName;
            Email = email;
            Name = name;
            Surname = surname;
            EmailConfirmed = emailConfirmed;
            PhoneNumber = phoneNumber;
            PhoneNumberConfirmed = phoneNumberConfirmed;
            TenantId = tenantId;
        }
    }
}