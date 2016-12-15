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

            Assert.Null(scopeProvider.CurrentScope?.Tenant);

            using (scopeProvider.EnterScope(new TenantInfo("A")))
            {
                Assert.Equal("A", scopeProvider.CurrentScope?.Tenant?.Id);

                using (scopeProvider.EnterScope(new TenantInfo("B")))
                {
                    Assert.Equal("B", scopeProvider.CurrentScope?.Tenant?.Id);

                    using (scopeProvider.EnterScope(null))
                    {
                        Assert.NotNull(scopeProvider.CurrentScope);
                        Assert.Null(scopeProvider.CurrentScope.Tenant);
                    }

                    Assert.Equal("B", scopeProvider.CurrentScope?.Tenant?.Id);
                }

                Assert.Equal("A", scopeProvider.CurrentScope?.Tenant?.Id);
            }

            scopeProvider.CurrentScope.ShouldBeNull();
        }
    }
}
