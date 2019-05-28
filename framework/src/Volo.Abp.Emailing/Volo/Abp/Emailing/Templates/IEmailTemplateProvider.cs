using System.Threading.Tasks;

namespace Volo.Abp.Emailing.Templates
{
    public interface IEmailTemplateProvider
    {
        Task<EmailTemplate> GetAsync(string name, string cultureName);
    }
}