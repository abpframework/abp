using Shouldly;
using Xunit;

namespace Volo.Abp.TextTemplating.Scriban;

public class ScribanTemplateRenderingEngine_IsSandboxed_Tests : AbpTextTemplatingTestBase<ScribanTextTemplatingTestModule>
{
    private readonly ScribanTemplateRenderingEngine _engine;

    public ScribanTemplateRenderingEngine_IsSandboxed_Tests()
    {
        _engine = GetRequiredService<ScribanTemplateRenderingEngine>();
    }

    [Fact]
    public void Scriban_Engine_Should_Be_Sandboxed()
    {
        // Scriban interprets templates as a restricted DSL without .NET interop;
        // editing template content is safe for non-developer users.
        _engine.IsSandboxed.ShouldBeTrue();
    }

    [Fact]
    public void Scriban_Engine_Should_Expose_IsSandboxed_Through_Interface()
    {
        ITemplateRenderingEngine asInterface = _engine;
        asInterface.IsSandboxed.ShouldBeTrue();
    }
}
