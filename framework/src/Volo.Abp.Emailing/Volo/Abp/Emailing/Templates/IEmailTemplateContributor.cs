namespace Volo.Abp.Emailing.Templates
{
    public interface IEmailTemplateContributor
    {
        void Initialize(EmailTemplateInitializationContext context);

        string GetOrNull(string cultureName);
    }
}