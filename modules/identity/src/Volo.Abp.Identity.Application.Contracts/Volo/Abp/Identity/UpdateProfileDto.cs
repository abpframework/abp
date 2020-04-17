﻿using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.Identity
{
    public class UpdateProfileDto : ExtensibleObject
    {
        [StringLength(IdentityUserConsts.MaxUserNameLength)]
        public string UserName { get; set; }

        [StringLength(IdentityUserConsts.MaxEmailLength)]
        public string Email { get; set; }

        [StringLength(IdentityUserConsts.MaxNameLength)]
        public string Name { get; set; }

        [StringLength(IdentityUserConsts.MaxSurnameLength)]
        public string Surname { get; set; }

        [StringLength(IdentityUserConsts.MaxPhoneNumberLength)]
        public string PhoneNumber { get; set; }
    }
}