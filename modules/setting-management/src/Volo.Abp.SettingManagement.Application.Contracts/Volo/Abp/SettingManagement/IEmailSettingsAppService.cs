using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Abp.SettingManagement;

public interface IEmailSettingsAppService : IApplicationService
{
    Task<EmailSettingsDto> GetAsync();

    Task UpdateAsync(UpdateEmailSettingsDto input);
}
