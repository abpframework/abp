using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;

namespace Volo.Abp.Auditing
{
    public class AbpAuditingOptions
    {
        //TODO: Consider to add an option to disable auditing for application service methods?

        /// <summary>
        /// Default: true.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Default: true.
        /// </summary>
        public bool IsEnabledForAnonymousUsers { get; set; }

        public List<AuditLogContributor> Contributors { get; }

        public List<Type> IgnoredTypes { get; }

        public IEntityHistorySelectorList EntityHistorySelectors { get; }
        
        public AbpAuditingOptions()
        {
            IsEnabled = true;
            IsEnabledForAnonymousUsers = true;

            Contributors = new List<AuditLogContributor>
            {
                new ClientInfoAuditLogContributor()
            };

            IgnoredTypes = new List<Type>
            {
                typeof(Stream),
                typeof(Expression)
            };

            EntityHistorySelectors = new EntityHistorySelectorList();
        }
    }
}