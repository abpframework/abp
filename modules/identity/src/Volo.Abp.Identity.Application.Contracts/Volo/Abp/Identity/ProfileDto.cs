﻿using Volo.Abp.ObjectExtending;

namespace Volo.Abp.Identity
{
    public class ProfileDto : ExtensibleObject
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsExternal { get; set; }

        public bool HasPassword { get; set; }
    }
}
