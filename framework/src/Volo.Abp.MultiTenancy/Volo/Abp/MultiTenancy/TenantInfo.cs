using System;
using JetBrains.Annotations;
using Volo.Abp.Data;

namespace Volo.Abp.MultiTenancy
{
    [Serializable]
    public class TenantInfo //TODO: Add a custom data to TenantInfo and make it available in ICurrentTenant
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ConnectionStrings ConnectionStrings { get; }

        public TenantInfo()
        {
            
        }

        public TenantInfo(Guid id, [NotNull] string name)
        {
            Check.NotNull(name, nameof(name));

            Id = id;
            Name = name;

            ConnectionStrings = new ConnectionStrings();
        }
    }
}