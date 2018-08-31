using System.Threading.Tasks;

namespace Volo.Abp.Emailing.Templates
{
    public interface IEmailTemplateProvider
    {
        Task ProvideAsync(EmailTemplateProviderContext context);
    }
}