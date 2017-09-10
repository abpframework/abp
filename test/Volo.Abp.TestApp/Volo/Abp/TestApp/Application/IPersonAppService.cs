using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Volo.Abp.TestApp.Application
{
    public interface IPersonAppService : IAsyncCrudAppService<PersonDto>
    {
        //URL: [GET] /api/people/{id}?type=office
        Task<ListResultDto<PhoneDto>> GetPhones(Guid id, GetPersonPhonesFilter filter);

        //URL: [POST] /api/people/{id}/phones
        Task<PhoneDto> AddPhone(Guid id, PhoneDto phoneDto);

        //URL: [DELETE] /api/people/{id}/phones/{phoneId}
        Task DeletePhone(Guid id, long phoneId);
    }
}