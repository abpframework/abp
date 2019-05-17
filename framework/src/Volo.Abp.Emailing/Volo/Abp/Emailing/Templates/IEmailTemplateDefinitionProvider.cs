namespace Volo.Abp.Emailing.Templates
{
    public interface IEmailTemplateDefinitionProvider
    {
        void Define(IEmailTemplateDefinitionContext context);
    }
}