using JetBrains.Annotations;

namespace Volo.Abp.MultiTenancy
{
    public class TenantInfo
    {
        [NotNull]
        public string Id { get; }

        private TenantInfo()
        {
            
        }

        public TenantInfo([NotNull] string id)
        {
            Check.NotNull(id, nameof(id));

            Id = id;
        }
    }
}