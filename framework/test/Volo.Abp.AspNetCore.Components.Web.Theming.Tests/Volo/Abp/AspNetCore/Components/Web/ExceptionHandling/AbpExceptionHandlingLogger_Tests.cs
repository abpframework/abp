using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shouldly;
using Volo.Abp.AspNetCore.Components.ExceptionHandling;
using Xunit;

namespace Volo.Abp.AspNetCore.Components.Web.ExceptionHandling;

public class AbpExceptionHandlingLogger_Tests
{
    [Fact]
    public void Should_Not_Recurse_When_Informer_Writes_To_Same_Logger_Pipeline()
    {
        var services = new ServiceCollection();
        var informer = new RecordingUserExceptionInformer();
        services.AddSingleton<IUserExceptionInformer>(informer);

        var provider = new AbpExceptionHandlingLoggerProvider(services);
        services.AddLogging(builder => builder
            .AddProvider(provider)
            .AddFilter<AbpExceptionHandlingLoggerProvider>(typeof(UserExceptionInformer).FullName, LogLevel.None));

        var accessor = services.AddObjectAccessor<IServiceProvider>();
        var sp = services.BuildServiceProvider();
        accessor.Value = sp;

        informer.Logger = sp.GetRequiredService<ILoggerFactory>()
            .CreateLogger(typeof(UserExceptionInformer).FullName!);

        var entryLogger = sp.GetRequiredService<ILogger<AbpExceptionHandlingLogger_Tests>>();
        entryLogger.LogError(new InvalidOperationException("boom"), "entry");

        informer.InformCalls.ShouldBe(1);
    }

    private sealed class RecordingUserExceptionInformer : IUserExceptionInformer
    {
        private const int RecursionCircuitBreaker = 50;

        public int InformCalls;
        public ILogger? Logger;

        public void Inform(UserExceptionInformerContext context)
        {
            InformCalls++;
            if (InformCalls > RecursionCircuitBreaker)
            {
                return;
            }

            Logger?.LogError(context.Exception, context.Exception.Message);
        }

        public Task InformAsync(UserExceptionInformerContext context)
        {
            Inform(context);
            return Task.CompletedTask;
        }
    }
}
