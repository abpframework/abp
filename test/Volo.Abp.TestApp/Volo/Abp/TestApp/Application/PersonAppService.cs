using Abp.Application.Services;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.TestApp.Application
{
    public class PersonAppService : AsyncCrudAppService<Person, PersonDto>, IPersonAppService
    {
        public PersonAppService(IQueryableRepository<Person> repository) 
            : base(repository)
        {
        }
    }
}
