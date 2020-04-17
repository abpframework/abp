﻿using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.Identity
{
    public abstract class IdentityUserCreateOrUpdateDtoBase : ExtensibleObject
    {
        [Required]
        [StringLength(IdentityUserConsts.MaxUserNameLength)]
        public string UserName { get; set; }

        [StringLength(IdentityUserConsts.MaxNameLength)]
        public string Name { get; set; }

        [StringLength(IdentityUserConsts.MaxSurnameLength)]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(IdentityUserConsts.MaxEmailLength)]
        public string Email { get; set; }

        [StringLength(IdentityUserConsts.MaxPhoneNumberLength)]
        public string PhoneNumber { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public bool LockoutEnabled { get; set; }

        [CanBeNull]
        public string[] RoleNames { get; set; }
    }
}