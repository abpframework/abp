using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Tracing
{
    public class NullCorrelationIdProvider : ICorrelationIdProvider, ISingletonDependency
    {
        public string Get()
        {
            return null;
        }
    }
}