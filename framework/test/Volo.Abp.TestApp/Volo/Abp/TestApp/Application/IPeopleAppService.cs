using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using Volo.Abp.TestApp.Application.Dto;

namespace Volo.Abp.TestApp.Application
{
    public interface IPeopleAppService : ICrudAppService<PersonDto, Guid>
    {
        Task<ListResultDto<PhoneDto>> GetPhones(Guid id, GetPersonPhonesFilter filter);

        Task<List<string>> GetPhones(GetPersonPhonesFilter input1, GetPersonPhonesFilter input2);

        Task<List<string>> GetParams(IEnumerable<Guid> ids, string[] names);

        Task<PhoneDto> AddPhone(Guid id, PhoneDto phoneDto);

        Task RemovePhone(Guid id, string number);

        Task GetWithAuthorized();

        Task<GetWithComplexTypeInput> GetWithComplexType(GetWithComplexTypeInput input);

        Task<IRemoteStreamContent> DownloadAsync();

        Task<string> UploadAsync(IRemoteStreamContent streamContent);

        Task<string> UploadMultipleAsync(IEnumerable<IRemoteStreamContent> streamContents);

        Task<string> CreateFileAsync(CreateFileInput input);

        Task<string> CreateMultipleFileAsync(CreateMultipleFileInput input);

    }
}
