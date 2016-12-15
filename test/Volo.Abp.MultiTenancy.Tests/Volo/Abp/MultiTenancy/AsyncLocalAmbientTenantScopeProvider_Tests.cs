using Shouldly;
using Xunit;

namespace Volo.Abp.MultiTenancy
{
    public class AsyncLocalAmbientTenantScopeProvider_Tests
    {
        [Fact]
        public void Should_Support_Inner_Scopes()
        {
            var scopeProvider = new AsyncLocalTenantScopeProvider();

            scopeProvider.CurrentScope.ShouldBeNull();

            using (scopeProvider.EnterScope(new TenantInfo("1","A")))
            {
                scopeProvider.CurrentScope.Tenant.Name.ShouldBe("A");

                using (scopeProvider.EnterScope(new TenantInfo("2", "B")))
                {
                    scopeProvider.CurrentScope.Tenant.Name.ShouldBe("B");

                    using (scopeProvider.EnterScope(null))
                    {
                        scopeProvider.CurrentScope.Tenant.ShouldBeNull();
                    }

                    scopeProvider.CurrentScope.Tenant.Name.ShouldBe("B");
                }

                scopeProvider.CurrentScope.Tenant.Name.ShouldBe("A");
            }

            scopeProvider.CurrentScope.ShouldBeNull();
        }
    }
}
