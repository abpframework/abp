using System;
using JetBrains.Annotations;

namespace Volo.Abp.MultiTenancy
{
    public class ActionTenantResolveContributer : ITenantResolveContributer
    {
        private readonly Action<ITenantResolveContext> _resolveAction;

        public ActionTenantResolveContributer([NotNull] Action<ITenantResolveContext> resolveAction)
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