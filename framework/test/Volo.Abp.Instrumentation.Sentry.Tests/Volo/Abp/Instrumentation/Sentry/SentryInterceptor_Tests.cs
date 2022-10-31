using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ClearExtensions;
using Sentry;
using Volo.Abp.Instrumentation.Sentry.TestServices;
using Xunit;

namespace Volo.Abp.Instrumentation.Sentry;

public class SentryInterceptor_Tests : SentryTestBase
{
    private readonly IMyInstrumentedBySentryService1 _myInstrumentedBySentryService1;
    public SentryInterceptor_Tests()
    {
        _myInstrumentedBySentryService1 = GetRequiredService<MyInstrumentedBySentryService1>();
    }

    [Fact]
    public async Task Should_StartChild()
    {
        // Arrange
        var parentSpan = Substitute.For<ISpan>();
        var childSpan = Substitute.For<ISpan>();
        parentSpan.StartChild(Arg.Any<string>()).Returns(childSpan);
        SentryHub.GetSpan().Returns(parentSpan);
        
        // Act 
        await _myInstrumentedBySentryService1.ClassHadSentrySpanButMethodNot();
        
        // Assert
        SentryHub.Received(1).GetSpan();
        parentSpan.Received(1).StartChild(Arg.Any<string>());
        Logger.Received(1).LogInformation($"Invoke {nameof(MyInstrumentedBySentryService1.ClassHadSentrySpanButMethodNot)}");
        childSpan.Received(1).Finish();
        Received.InOrder(() =>
        {
            SentryHub.GetSpan();
            parentSpan.StartChild(Arg.Any<string>());
            Logger.LogInformation($"Invoke {nameof(MyInstrumentedBySentryService1.ClassHadSentrySpanButMethodNot)}");
            childSpan.Finish();
        });
    }

    [Fact]
    public async Task Should_StartChild_And_InvokedMethod_Should_StartChild()
    {
        // Arrange
        var parentSpan = Substitute.For<ISpan>();
        
        var childSpan = Substitute.For<ISpan>();
        parentSpan.StartChild(Arg.Any<string>()).Returns(childSpan);
        
        var invokeMethodSpan = Substitute.For<ISpan>();
        childSpan.StartChild(Arg.Any<string>()).Returns(invokeMethodSpan);
        
        SentryHub.GetSpan().Returns(parentSpan, childSpan);

        
        // Act 
        await _myInstrumentedBySentryService1.ClassHadSentrySpanButMethodNotWillInvokeNextMethod();
        
        // Assert
        SentryHub.Received(2).GetSpan();
        parentSpan.Received(1).StartChild(Arg.Any<string>());
        childSpan.Received(1).Finish();
        childSpan.Received(1).StartChild(Arg.Any<string>());
        invokeMethodSpan.Received(1).Finish();
        Received.InOrder(() =>
        {
            SentryHub.GetSpan();
            parentSpan.StartChild(Arg.Any<string>());
            SentryHub.GetSpan();
            childSpan.StartChild(Arg.Any<string>());
            invokeMethodSpan.Finish();
            childSpan.Finish();
        });
        
    }
}