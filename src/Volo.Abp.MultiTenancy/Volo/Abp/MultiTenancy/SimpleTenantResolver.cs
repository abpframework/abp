using System;
using JetBrains.Annotations;

namespace Volo.Abp.MultiTenancy
{
    public class SimpleTenantResolver : ITenantResolver
    {
        private readonly Action<ITenantResolveContext> _resolveAction;

        public SimpleTenantResolver([NotNull] Action<ITenantResolveContext> resolveAction)
        {
            Check.NotNull(resolveAction, nameof(resolveAction));

            _resolveAction = resolveAction;
        }

        public void Resolve(ITenantResolveContext context)
        {
            _resolveAction(context);
        }
    }
}