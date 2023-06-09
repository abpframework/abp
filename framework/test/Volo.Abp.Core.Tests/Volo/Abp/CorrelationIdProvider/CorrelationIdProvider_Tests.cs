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
            var currentCorrelationId = correlationIdProvider.Get();
            currentCorrelationId.ShouldNotBeNull();

            var correlationId = Guid.NewGuid().ToString("N");
            using (correlationIdProvider.Change(correlationId))
            {
                correlationIdProvider.Get().ShouldBe(correlationId);
            }

            //The default correlation id always changes.
            correlationIdProvider.Get().ShouldNotBe(currentCorrelationId);
        }
    }
}
