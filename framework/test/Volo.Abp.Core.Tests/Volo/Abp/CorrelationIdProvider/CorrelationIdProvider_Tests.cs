using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Abp.Tracing;
using Xunit;

namespace Volo.Abp.CorrelationIdProvider;

public class CorrelationIdProvider_Tests
{
    [Fact]
    public async Task Test()
    {
        using (var application = await AbpApplicationFactory.CreateAsync<IndependentEmptyModule>())
        {
            await application.InitializeAsync();

            var correlationIdProvider = application.ServiceProvider.GetRequiredService<ICorrelationIdProvider>();

            correlationIdProvider.Get().ShouldBeNull();

            var correlationId = Guid.NewGuid().ToString("N");
            using (correlationIdProvider.Change(correlationId))
            {
                correlationIdProvider.Get().ShouldBe(correlationId);
            }

            correlationIdProvider.Get().ShouldBeNull();
        }
    }
}
