using System.Threading.Tasks;
using global::Scriban.Syntax;
using Shouldly;
using Xunit;

namespace Volo.Abp.TextTemplating.Scriban;

public class ScribanTemplateRenderingEngine_IsSandboxed_Tests : AbpTextTemplatingTestBase<ScribanTextTemplatingTestModule>
{
    private readonly ScribanTemplateRenderingEngine _engine;
    private readonly ITemplateRenderer _templateRenderer;

    public ScribanTemplateRenderingEngine_IsSandboxed_Tests()
    {
        _engine = GetRequiredService<ScribanTemplateRenderingEngine>();
        _templateRenderer = GetRequiredService<ITemplateRenderer>();
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

    [Fact]
    public async Task Should_Hide_GetType_Member_On_Imported_Model()
    {
        // The engine projects .NET model objects into a ScriptObject containing
        // only the model's public readable properties. Reflection entry points
        // such as object.GetType are not part of the projection, so
        // "{{ model.GetType }}" silently resolves to null (empty output) instead
        // of leaking the runtime type.
        var result = await _templateRenderer.RenderAsync(
            ScribanTestTemplateDefinitionProvider.ReflectionEscapeAttempt,
            model: new ReflectionEscapeModel { Name = "John" });

        result.ShouldBe("getType=[]\n");
        result.ShouldNotContain("RuntimeType");
        result.ShouldNotContain("Volo.Abp.TextTemplating");
    }

    [Fact]
    public async Task Should_Block_Reflection_Escape_Chain()
    {
        // Direct reflection chain that an attacker would attempt:
        //   {{ model.GetType.Assembly.GetType "System.IO.File" }}
        // Because the projected ScriptObject does not expose GetType, the first
        // hop yields null and Scriban raises a ScriptRuntimeException when the
        // chain dereferences a null. Surfacing an error is desirable: silent
        // failure could mask escape attempts.
        var ex = await Should.ThrowAsync<ScriptRuntimeException>(async () =>
            await _templateRenderer.RenderAsync(
                ScribanTestTemplateDefinitionProvider.ReflectionEscapeChain,
                model: new ReflectionEscapeModel { Name = "John" }));

        // The error must reference the broken chain (null), not a leaked Type
        // or Assembly value.
        ex.Message.ShouldContain("null");
        ex.Message.ShouldNotContain("System.IO.File");
        ex.Message.ShouldNotContain("System.Private.CoreLib");
    }

    [Fact]
    public async Task Should_Hide_Methods_Of_Imported_Model()
    {
        // Methods on .NET objects are not exposed by the projection so
        // attackers cannot invoke side-effecting methods (e.g. repository
        // mutators) even when the host accidentally imports a service-like
        // object as a model.
        var result = await _templateRenderer.RenderAsync(
            ScribanTestTemplateDefinitionProvider.MethodInvocationAttempt,
            model: new ServiceLikeModel());

        result.ShouldBe("danger=[]\n");
        result.ShouldNotContain("UNSAFE");
    }

    [Fact]
    public async Task Should_Project_Properties_Including_Nested_Objects()
    {
        // The projection must still expose user-defined readable properties
        // (and recurse into nested objects) so legitimate template usage keeps
        // working after sandboxing.
        var result = await _templateRenderer.RenderAsync(
            ScribanTestTemplateDefinitionProvider.NestedPropertyAccess,
            model: new OuterModel { Name = "John", Inner = new InnerModel { Email = "j@a.b" } });

        result.ShouldBe("name=[John] email=[j@a.b]\n");
    }

    private class ReflectionEscapeModel
    {
        public string Name { get; set; } = default!;
    }

    private class ServiceLikeModel
    {
        public string DangerousAction()
        {
            return "UNSAFE";
        }
    }

    private class OuterModel
    {
        public string Name { get; set; } = default!;
        public InnerModel Inner { get; set; } = default!;
    }

    private class InnerModel
    {
        public string Email { get; set; } = default!;
    }
}
