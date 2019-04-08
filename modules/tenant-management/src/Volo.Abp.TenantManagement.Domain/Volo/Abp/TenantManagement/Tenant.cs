using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace Volo.Abp.TenantManagement
{
    public class Tenant : FullAuditedAggregateRoot<Guid>
    {
        public virtual string Name { get; protected set; }

        public virtual List<TenantConnectionString> ConnectionStrings { get; protected set; }

        protected Tenant()
        {
            ExtraProperties = new Dictionary<string, object>();
        }

        protected internal Tenant(Guid id, [NotNull] string name)
        {
            Id = id;
            SetName(name);

            ConnectionStrings = new List<TenantConnectionString>();
            ExtraProperties = new Dictionary<string, object>();
        }

        [CanBeNull]
        public virtual string FindDefaultConnectionString()
        {
            return FindConnectionString(Data.ConnectionStrings.DefaultConnectionStringName);
        }

        [CanBeNull]
        public virtual string FindConnectionString(string name)
        {
            return ConnectionStrings.FirstOrDefault(c => c.Name == name)?.Value;
        }

        internal void SetName([NotNull] string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), TenantConsts.MaxNameLength);
        }
    }
}