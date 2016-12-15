using JetBrains.Annotations;

namespace Volo.Abp.MultiTenancy
{
    public class TenantInfo
    {
        [NotNull]
        public string Id { get; }

        //TODO: Needed for serialization
        [UsedImplicitly]
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