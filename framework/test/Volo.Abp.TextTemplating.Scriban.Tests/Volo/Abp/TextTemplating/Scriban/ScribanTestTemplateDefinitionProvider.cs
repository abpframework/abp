namespace Volo.Abp.TextTemplating.Scriban;

public class ScribanTestTemplateDefinitionProvider : TemplateDefinitionProvider
{
    public const string ReflectionEscapeAttempt = "ReflectionEscapeAttempt";
    public const string ReflectionEscapeChain = "ReflectionEscapeChain";
    public const string MethodInvocationAttempt = "MethodInvocationAttempt";
    public const string NestedPropertyAccess = "NestedPropertyAccess";

    public override void Define(ITemplateDefinitionContext context)
    {
        context.GetOrNull(TestTemplates.WelcomeEmail)?
            .WithVirtualFilePath("/SampleTemplates/WelcomeEmail", false)
            .WithScribanEngine();

        context.GetOrNull(TestTemplates.ForgotPasswordEmail)?
            .WithVirtualFilePath("/SampleTemplates/ForgotPasswordEmail.tpl", true)
            .WithScribanEngine();

        context.GetOrNull(TestTemplates.TestTemplateLayout1)?
            .WithVirtualFilePath("/SampleTemplates/TestTemplateLayout1.tpl", true)
            .WithScribanEngine();

        context.GetOrNull(TestTemplates.ShowDecimalNumber)?
            .WithVirtualFilePath("/SampleTemplates/ShowDecimalNumber.tpl", true)
            .WithScribanEngine();

        context.Add(new TemplateDefinition(ReflectionEscapeAttempt)
            .WithVirtualFilePath("/SampleTemplates/ReflectionEscapeAttempt.tpl", true)
            .WithScribanEngine());

        context.Add(new TemplateDefinition(ReflectionEscapeChain)
            .WithVirtualFilePath("/SampleTemplates/ReflectionEscapeChain.tpl", true)
            .WithScribanEngine());

        context.Add(new TemplateDefinition(MethodInvocationAttempt)
            .WithVirtualFilePath("/SampleTemplates/MethodInvocationAttempt.tpl", true)
            .WithScribanEngine());

        context.Add(new TemplateDefinition(NestedPropertyAccess)
            .WithVirtualFilePath("/SampleTemplates/NestedPropertyAccess.tpl", true)
            .WithScribanEngine());
    }
}
