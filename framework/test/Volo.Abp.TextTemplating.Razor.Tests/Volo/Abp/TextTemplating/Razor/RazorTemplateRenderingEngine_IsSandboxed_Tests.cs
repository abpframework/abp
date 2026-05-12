using Shouldly;
using Xunit;

namespace Volo.Abp.TextTemplating.Razor;

public class RazorTemplateRenderingEngine_IsSandboxed_Tests : AbpTextTemplatingTestBase<RazorTextTemplatingTestModule>
{
    private readonly RazorTemplateRenderingEngine _engine;

    public RazorTemplateRenderingEngine_IsSandboxed_Tests()
    {
        _engine = GetRequiredService<RazorTemplateRenderingEngine>();
    }

    [Fact]
    public void Razor_Engine_Should_Not_Be_Sandboxed()
    {
        // Razor templates compile into fully-trusted .NET code; editing them is
        // equivalent to granting server-side code execution.
        _engine.IsSandboxed.ShouldBeFalse();
    }

    [Fact]
    public void Razor_Engine_Should_Expose_IsSandboxed_Through_Interface()
    {
        ITemplateRenderingEngine asInterface = _engine;
        asInterface.IsSandboxed.ShouldBeFalse();
    }
}
