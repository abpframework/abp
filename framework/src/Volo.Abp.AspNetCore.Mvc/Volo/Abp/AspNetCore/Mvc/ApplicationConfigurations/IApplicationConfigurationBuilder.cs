using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations
{
    public interface IApplicationConfigurationBuilder
    {
        Task<ApplicationConfigurationDto> GetAsync();
    }
}