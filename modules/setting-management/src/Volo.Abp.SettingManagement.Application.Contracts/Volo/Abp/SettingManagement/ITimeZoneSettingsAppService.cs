using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Abp.SettingManagement;

public interface ITimeZoneSettingsAppService : IApplicationService
{
    Task<string> GetAsync();

    Task<List<NameValue>> GetTimezonesAsync();

    Task UpdateAsync(string timezone);
}
