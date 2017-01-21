using System;
using JetBrains.Annotations;
using Volo.Abp.Data;

namespace Volo.Abp.MultiTenancy
{
    public class TenantInformation
    {
        public Guid Id { get; protected set; }

        public string Name { get; protected set; }

        public ConnectionStrings ConnectionStrings { get; protected set; }

        protected TenantInformation()
        {
            
        }

        public TenantInformation(Guid id, [NotNull] string name)
        {
            Id = id;
            Name = name;

            ConnectionStrings = new ConnectionStrings();
        }
    }
}