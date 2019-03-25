using System;
using JetBrains.Annotations;

namespace Volo.Abp.MultiTenancy
{
    public class ActionTenantResolveContributor : ITenantResolveContributor
    {
        private readonly Action<ITenantResolveContext> _resolveAction;

        public ActionTenantResolveContributor([NotNull] Action<ITenantResolveContext> resolveAction)
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