using System;
using System.Security.Claims;
using System.Threading;
using System.Xml.Schema;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Security.Claims
{
    public class ThreadCurrentPrincipalAccessor : ICurrentPrincipalAccessor, ISingletonDependency
    {
        public Guid Id = Guid.NewGuid();

        public virtual ClaimsPrincipal Principal
        {
            get => CurrentScope.Value?.Invoke();
            set => CurrentScope.Value = () => value;
        }

        protected readonly AsyncLocal<Func<ClaimsPrincipal>> CurrentScope;

        public ThreadCurrentPrincipalAccessor()
        {
            CurrentScope = new AsyncLocal<Func<ClaimsPrincipal>>
            {
                Value = GetThreadClaimsPrincipal
            };
        }

        protected ClaimsPrincipal GetThreadClaimsPrincipal()
        {
            return Thread.CurrentPrincipal as ClaimsPrincipal;
        }

        public virtual IDisposable Change(ClaimsPrincipal principal)
        {
            var parentScope = Principal;
            Principal = principal;
            return new DisposeAction(() =>
            {
                Principal = parentScope;
            });
        }
    }
}
