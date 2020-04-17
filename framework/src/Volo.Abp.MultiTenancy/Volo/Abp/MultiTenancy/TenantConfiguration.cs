using System;
using JetBrains.Annotations;
using Volo.Abp.Data;

namespace Volo.Abp.MultiTenancy
{
    [Serializable]
    public class TenantConfiguration
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ConnectionStrings ConnectionStrings { get; set; }

        public TenantConfiguration()
        {
            
        }

        public TenantConfiguration(Guid id, [NotNull] string name)
        {
            Check.NotNull(name, nameof(name));

            Id = id;
            Name = name;

            ConnectionStrings = new ConnectionStrings();
        }
    }
}