using System.Threading.Tasks;

namespace Volo.Abp.Http.Modeling;

public interface IApiDescriptionModelProvider
{
    Task<ApplicationApiDescriptionModel> CreateApiModelAsync(ApplicationApiDescriptionModelRequestDto input);
}
