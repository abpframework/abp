using System;
using Microsoft.AspNetCore.Http;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Auditing
{
    [Dependency(ReplaceServices = true)]
    public class AspNetCoreCorrelationIdProvider : ICorrelationIdProvider, ITransientDependency
    {
        public const string CorrelationIdKey = "_CorrelationId";

        protected IHttpContextAccessor HttpContextAccessor { get; }

        public AspNetCoreCorrelationIdProvider(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public virtual string Get()
        {
            if (HttpContextAccessor.HttpContext?.Request?.Headers == null)
            {
                return CreateNewCorrelationId();
            }

            string correlationId = HttpContextAccessor.HttpContext.Request.Headers[CorrelationIdKey];

            if (correlationId.IsNullOrEmpty())
            {
                correlationId = CreateNewCorrelationId();
                HttpContextAccessor.HttpContext.Request.Headers[CorrelationIdKey] = correlationId;
            }

            return correlationId;
        }

        protected virtual string CreateNewCorrelationId()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
