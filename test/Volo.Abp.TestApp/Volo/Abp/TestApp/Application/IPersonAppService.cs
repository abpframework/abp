using Abp.Application.Services;

namespace Volo.Abp.TestApp.Application
{
    public interface IPersonAppService : IAsyncCrudAppService<PersonDto>
    {
    }
}