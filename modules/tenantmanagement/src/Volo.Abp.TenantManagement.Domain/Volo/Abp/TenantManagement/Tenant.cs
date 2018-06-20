using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.TenantManagement
{
    public class Tenant : AggregateRoot<Guid>, IHasExtraProperties
    {
        public virtual string Name { get; protected set; }

        public virtual List<TenantConnectionString> ConnectionStrings { get; protected set; }

        public Dictionary<string, object> ExtraProperties { get; }

        protected Tenant()
        {
            ExtraProperties = new Dictionary<string, object>();
        }

        protected internal Tenant(Guid id, [NotNull] string name)
        {
            Check.NotNull(name, nameof(name));

            Id = id;
            Name = name;

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
            Check.NotNull(name, nameof(name));

            Name = name;
        }
    }
}