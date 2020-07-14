using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Samples
{
    public interface ISampleAppService : IApplicationService
    {
        Task<SampleDto> GetAsync();

        Task<SampleDto> GetAuthorizedAsync();
    }
}
