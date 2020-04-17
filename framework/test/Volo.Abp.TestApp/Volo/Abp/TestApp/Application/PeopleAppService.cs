using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Application.Services;
using Volo.Abp.TestApp.Application.Dto;

namespace Volo.Abp.TestApp.Application
{
    public class PeopleAppService : CrudAppService<Person, PersonDto, Guid>, IPeopleAppService
    {
        public PeopleAppService(IRepository<Person, Guid> repository)
            : base(repository)
        {

        }
        
        public async Task<ListResultDto<PhoneDto>> GetPhones(Guid id, GetPersonPhonesFilter filter)
        {
            var phones = (await GetEntityByIdAsync(id)).Phones
                .WhereIf(filter.Type.HasValue, p => p.Type == filter.Type)
                .ToList();
            
            return new ListResultDto<PhoneDto>(
                ObjectMapper.Map<List<Phone>, List<PhoneDto>>(phones)
            );
        }

        public async Task<PhoneDto> AddPhone(Guid id, PhoneDto phoneDto)
        {
            var person = await GetEntityByIdAsync(id);
            var phone = new Phone(person.Id, phoneDto.Number, phoneDto.Type);

            person.Phones.Add(phone);
            await Repository.UpdateAsync(person);
            return ObjectMapper.Map<Phone, PhoneDto>(phone);
        }

        public async Task RemovePhone(Guid id, string number)
        {
            var person = await GetEntityByIdAsync(id);
            person.Phones.RemoveAll(p => p.Number == number);
            await Repository.UpdateAsync(person);
        }

        [Authorize]
        public Task GetWithAuthorized()
        {
            return Task.CompletedTask;
        }

        public Task<GetWithComplexTypeInput> GetWithComplexType(GetWithComplexTypeInput input)
        {
            return Task.FromResult(input);
        }
    }
}
