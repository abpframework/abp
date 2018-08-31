using System.Threading.Tasks;

namespace Volo.Abp.Emailing.Templates
{
    public interface IEmailTemplateStore
    {
        Task<EmailTemplate> GetAsync(string name);
    }
}
