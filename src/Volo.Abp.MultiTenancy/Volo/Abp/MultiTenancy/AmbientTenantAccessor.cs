using System.Threading;
using Volo.DependencyInjection;

namespace Volo.Abp.MultiTenancy
{
    public class AmbientTenantAccessor : IAmbientTenantAccessor, ISingletonDependency //TODO: Should be IScopedDependency?
    {
        public AmbientTenantInfo AmbientTenant
        {
            get { return _tenant.Value; }
            set { _tenant.Value = value; }
        }

        private readonly AsyncLocal<AmbientTenantInfo> _tenant;

        public AmbientTenantAccessor()
        {
            _tenant = new AsyncLocal<AmbientTenantInfo>();
        }
    }
}