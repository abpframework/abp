using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.MultiTenancy
{
    public class Tenant : AggregateRoot<Guid>
    {
        public virtual string Name { get; protected set; }

        public virtual List<TenantConnectionString> ConnectionStrings { get; protected set; }

        protected Tenant()
        {
            
        }

        public Tenant(Guid id, [NotNull] string name)
        {
            Id = id;
            Name = name;

            ConnectionStrings = new List<TenantConnectionString>();
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
    }
}