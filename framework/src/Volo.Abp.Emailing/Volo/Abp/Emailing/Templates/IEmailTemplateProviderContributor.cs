using System.Threading.Tasks;

namespace Volo.Abp.Emailing.Templates
{
    public interface IEmailTemplateProviderContributor
    {
        Task ProvideAsync(EmailTemplateProviderContributorContext contributorContext);
    }
}