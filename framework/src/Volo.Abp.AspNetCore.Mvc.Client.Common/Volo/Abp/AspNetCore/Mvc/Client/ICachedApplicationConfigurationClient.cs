using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

namespace Volo.Abp.AspNetCore.Mvc.Client
{
    public interface ICachedApplicationConfigurationClient
    {
        Task<ApplicationConfigurationDto> GetAsync();
    }
}