using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Volo.Abp.TestApp.Application
{
    public interface IPeopleAppService : IAsyncCrudAppService<PersonDto>
    {
        Task<ListResultDto<PhoneDto>> GetPhones(Guid id, GetPersonPhonesFilter filter);

        Task<PhoneDto> AddPhone(Guid id, PhoneDto phoneDto);

        Task RemovePhone(Guid id, long phoneId);

        Task<GetWithComplexTypeInput> GetWithComplexType(GetWithComplexTypeInput input);
    }

    public class GetWithComplexTypeInput
    {
        public string Value1 { get; set; }

        public GetWithComplexTypeInner Inner1 { get; set; }
    }

    public class GetWithComplexTypeInner
    {
        public string Value2 { get; set; }
        public GetWithComplexTypeInnerInner Inner2 { get; set; }
    }

    public class GetWithComplexTypeInnerInner
    {
        public string Value3 { get; set; }
    }
}