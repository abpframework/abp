﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.TestApp.Application.Dto;

namespace Volo.Abp.TestApp.Application
{
    public interface IPeopleAppService : ICrudAppService<PersonDto, Guid>
    {
        Task<ListResultDto<PhoneDto>> GetPhones(Guid id, GetPersonPhonesFilter filter);

        Task<List<string>> GetParams(IEnumerable<Guid> ids, string[] names);

        Task<List<string>> GetDictionaryParams(Dictionary<int, string> dic);

        Task<PhoneDto> AddPhone(Guid id, PhoneDto phoneDto);

        Task RemovePhone(Guid id, string number);

        Task GetWithAuthorized();

        Task<GetWithComplexTypeInput> GetWithComplexType(GetWithComplexTypeInput input);
    }
}
