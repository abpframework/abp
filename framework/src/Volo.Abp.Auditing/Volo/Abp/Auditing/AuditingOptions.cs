using System;
using System.Collections.Generic;

namespace Volo.Abp.Auditing
{
    public class AuditingOptions
    {
        public bool IsEnabled { get; set; }

        public bool IsEnabledForAnonymousUsers { get; set; }

        public List<Type> IgnoredTypes { get; }

        public AuditingOptions()
        {
            IsEnabled = true;
            IsEnabledForAnonymousUsers = true;
            IgnoredTypes = new List<Type>();
        }
    }
}