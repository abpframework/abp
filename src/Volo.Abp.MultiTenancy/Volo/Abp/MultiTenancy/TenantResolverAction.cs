using System;
using JetBrains.Annotations;

namespace Volo.Abp.MultiTenancy
{
    public class TenantResolverAction : ITenantResolver
    {
        private readonly Action<ICurrentTenantResolveContext> _resolveAction;

        public TenantResolverAction([NotNull] Action<ICurrentTenantResolveContext> resolveAction)
        {
            Check.NotNull(resolveAction, nameof(resolveAction));

            _resolveAction = resolveAction;
        }

        public void Resolve(ICurrentTenantResolveContext context)
        {
            _resolveAction(context);
        }
    }
}