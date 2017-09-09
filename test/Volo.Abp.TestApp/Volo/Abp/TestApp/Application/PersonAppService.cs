using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Application.Services;

namespace Volo.Abp.TestApp.Application
{
    public class PersonAppService : AsyncCrudAppService<Person, PersonDto>, IPersonAppService
    {
        public PersonAppService(IQueryableRepository<Person> repository) 
            : base(repository)
        {

        }
        
        public async Task<ListResultDto<PhoneDto>> GetPhones(Guid id, GetPersonPhonesFilter input)
        {
            var person = await GetEntityByIdAsync(id);

            return new ListResultDto<PhoneDto>(
                ObjectMapper.Map<Collection<Phone>, Collection<PhoneDto>>(person.Phones)
            );
        }
    }
}
