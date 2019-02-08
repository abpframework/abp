using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Auditing
{
    public class NullCorrelationIdProvider : ICorrelationIdProvider, ISingletonDependency
    {
        public string Get()
        {
            return null;
        }
    }
}