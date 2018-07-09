using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;

namespace Volo.Abp.Auditing
{
    public class AuditingOptions
    {
        public bool IsEnabled { get; set; }

        public bool IsEnabledForAnonymousUsers { get; set; }

        public List<IAuditInfoContributor> Contributors { get; }

        public List<Type> IgnoredTypes { get; }

        public AuditingOptions()
        {
            IsEnabled = true;
            IsEnabledForAnonymousUsers = true;

            Contributors = new List<IAuditInfoContributor>
            {
                new ClientInfoAuditInfoContributor()
            };

            IgnoredTypes = new List<Type>
            {
                typeof(Stream),
                typeof(Expression)
            };
        }
    }
}