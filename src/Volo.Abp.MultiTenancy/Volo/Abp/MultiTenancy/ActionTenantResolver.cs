using System;
using JetBrains.Annotations;

namespace Volo.Abp.MultiTenancy
{
    public class ActionTenantResolver : ITenantResolver
    {
        private readonly Action<ITenantResolveContext> _resolveAction;

        public ActionTenantResolver([NotNull] Action<ITenantResolveContext> resolveAction)
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