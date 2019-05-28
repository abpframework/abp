using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Emailing.Templates
{
    public abstract class EmailTemplateDefinitionProvider : IEmailTemplateDefinitionProvider, ITransientDependency
    {
        public abstract void Define(IEmailTemplateDefinitionContext context);
    }
}