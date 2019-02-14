using System;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Tracing
{
    public class DefaultCorrelationIdProvider : ICorrelationIdProvider, ISingletonDependency
    {
        public string Get()
        {
            return CreateNewCorrelationId();
        }

        protected virtual string CreateNewCorrelationId()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}