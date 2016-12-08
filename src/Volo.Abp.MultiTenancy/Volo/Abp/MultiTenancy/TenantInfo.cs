using JetBrains.Annotations;

namespace Volo.Abp.MultiTenancy
{
    public class TenantInfo
    {
        public string Id { get; }

        public string Name { get; }

        private TenantInfo()
        {
            
        }

        public TenantInfo([NotNull] string id, [NotNull] string name)
        {
            Check.NotNull(id, nameof(id));
            Check.NotNull(name, nameof(name));

            Id = id;
            Name = name;
        }
    }
}